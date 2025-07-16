using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Handler;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Models.ChatHistory;
using EY.CE.Copilot.API.Static;
using EY.CE.Copilot.Data.Contexts;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using static EY.CE.Copilot.API.Models.Chat;
using static EY.CE.Copilot.API.Static.Constants;

namespace EY.CE.Copilot.API.Clients
{
    public class ChatClient : BaseClass, IChatClient
    {
        private readonly IConfiguration configuration;
        private readonly IOrchestratorClient orchestrator;
        private readonly ISessionClient _sessionClient;
        private readonly IDocumentsClient documentsClient;
        private readonly IPortalClient _portalClient;
        private readonly IExportFileClient _exportFileClient;
        private readonly string baseUrl;
        private readonly ISuggestionsClient suggestionsClient;
        private readonly CopilotContext context;
        private readonly HttpContext httpContext;
        private readonly object? userId;
        private readonly string? idpToken;
        private readonly IFireForgetRepositoryHandler fireForgetRepositoryHandler;
        List<SourceConfiguration> sourceConfigurations = new List<SourceConfiguration>();
        private readonly string UserEmail;

        public ChatClient(
            IConfiguration configuration,
            IOrchestratorClient orchestrator,
            ISessionClient session,
            IPortalClient portalClient,
            CopilotContext context,
            IHttpContextAccessor httpContextAccessor,
            IAppLoggerService logger,
            ISuggestionsClient suggestionsClient,
            IFireForgetRepositoryHandler _fireForgetRepositoryHandler,
            IDocumentsClient documentsClient,
            IExportFileClient exportFileClient) : base(logger, nameof(ChatClient))
        {
            this.configuration = configuration;
            this.orchestrator = orchestrator;
            _sessionClient = session;
            _portalClient = portalClient;
            baseUrl = configuration[SharedKeyVault.ORCHESTRATOR_BASE_URL];
            this.suggestionsClient = suggestionsClient;
            this.context = context;
            httpContext = httpContextAccessor.HttpContext;
            httpContext?.Items.TryGetValue(Constants.UserMail, out userId);
            idpToken = httpContext?.Request.Headers[Constants.IDPAuthorizationHeader];
            fireForgetRepositoryHandler = _fireForgetRepositoryHandler;
            this.documentsClient = documentsClient;
            _exportFileClient = exportFileClient;
           
            if (httpContext.Items.TryGetValue(Constants.UserMail, out object? mail))
                UserEmail = mail.ToString();

        }
        public async Task<string> StartChat()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<IActionResult> PostChat(string token, Chat.Input input)
        {
            try
            {
                var apps = await GetAppsfromCacheorPortal();
                input.Context.ActivePOApps = apps;
                if (!string.IsNullOrWhiteSpace(input.Context?.Suggestion?.Id))
                {
                    int id = Convert.ToInt32(input.Context.Suggestion.Id);
                    Data.Models.Suggestion suggestion = await suggestionsClient.Get(id);
                    //if suggestion has value then add to payload for orchestrator
                    if ((suggestion != null) && (!string.IsNullOrEmpty(suggestion.AnswerSQL)))
                    {
                        input.Context.Suggestion.SqlQuery = suggestion.AnswerSQL;
                    }
                }

                if (input.Sources.Contains(Constants.Chats.ProjectDocsSource))
                {
                    string filter = SharePoint.DocumentsODataFilterVisibleToAssistant;
                    DocsRequest request = new DocsRequest { Filter = filter, GeneratorType = GeneratorType.ChatRequest, User = UserEmail};
                    var projectDocs = await documentsClient.GetProjectDocs(request,true);
                    if (projectDocs != null && projectDocs.Any())
                    {
                        input.Context ??= new Context();
                        input.Context.ExcludeDocuments ??= new List<string>();
                        input.Context.Documents = projectDocs.Where(doc => { return !input.Context.ExcludeDocuments.Any(exclude => exclude == doc.ID); }).Select(doc => doc.ID);
                    }
                }
                input.InputSources = input.Sources;

                string payload = JsonConvert.SerializeObject(input);

                // to be removed after orchestrator is updated
                payload = payload.Replace(",\"isMessageLiked\":null", string.Empty);

                var response = await orchestrator.PostRequest(token, OrchestratorEndpoints.PostChat, payload);
                string? content = ((ContentResult)response).Content;
                Chat.OrchestratorStatus chatResponse = JsonConvert.DeserializeObject<Chat.OrchestratorStatus>(content);
                await SaveQuestion(input, chatResponse);
                return response;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method PostChat - {e.Message}", nameof(PostChat), exception: e);
                throw;
            }
        }

        public async Task<IActionResult> GetStatus(string token, Chat.Status input, string chatId)
        {
            try
            {
                string key = string.Format(Constants.Redis.Keys.MessageKey, userId.ToString(), chatId);
                string endpoint = input.Uri;
                if (endpoint.ToLower().Contains(baseUrl))
                {
                    endpoint = endpoint.Replace(baseUrl, string.Empty);
                }

                var response = await orchestrator.GetRequest(token, endpoint);

                if (response is ContentResult contentResult)
                {
                    int statusCode = (int)contentResult.StatusCode;

                    //If Status code is 200 then save the response in redis
                    if (contentResult.StatusCode == StatusCodes.Status200OK)
                    {
                        string? content = ((ContentResult)response).Content;

                        Chat.ResponseObject chatResponse = JsonConvert.DeserializeObject<Chat.ResponseObject>(content);
                        TimeSpan? ts = (chatResponse.LastUpdatedTime - chatResponse.CreatedTime);
                        chatResponse.Output.InstanceId = chatResponse.InstanceId;
                        if (chatResponse.RuntimeStatus.ToLower().Equals(RunTimeStatus.Completed))
                        {
                            string contentSummary = GenerateContentFromResponse(chatResponse.Output.Response, chatResponse.Input);

                            if (!string.IsNullOrWhiteSpace(contentSummary))
                            {
                                chatResponse.Output.Content = contentSummary;
                            }

                            SaveResponseToRedis(key, Chat.ConvertOutputToMessage(chatResponse), RunTimeStatus.Completed, StatusCodes.Status200OK);
                            UpdateChatInSQL(chatId, Chat.ConvertOutputToMessage(chatResponse), content, ts, chatResponse.RuntimeStatus, StatusCodes.Status200OK);
                            return new ContentResult
                            {
                                Content = JsonConvert.SerializeObject(chatResponse.Output),
                                ContentType = Static.Constants.ContentType.TextPlain,
                                StatusCode = StatusCodes.Status200OK
                            };
                        }
                        else if (chatResponse.RuntimeStatus.ToLower().Equals(RunTimeStatus.Failed))
                        {
                            chatResponse.Output.MessageId = ++chatResponse.Input.MessageId;
                            chatResponse.Output.Content = Constants.Chats.Message.RetryFailed;
                            chatResponse.Output.Role = Role.Assistant;
                            chatResponse.Output.LastUpdatedTime = DateTime.UtcNow;
                            SaveResponseToRedis(key, Chat.ConvertOutputToMessage(chatResponse), RunTimeStatus.Failed, StatusCodes.Status500InternalServerError);
                            UpdateChatInSQL(chatId, Chat.ConvertOutputToMessage(chatResponse), content, ts, chatResponse.RuntimeStatus, StatusCodes.Status500InternalServerError);
                            return new ContentResult
                            {
                                Content = JsonConvert.SerializeObject(chatResponse.Output),
                                ContentType = Static.Constants.ContentType.TextPlain,
                                StatusCode = StatusCodes.Status500InternalServerError
                            };
                        }
                    }
                    else
                    {
                        Chat.ResponseObject chatResponse = new ResponseObject();
                        chatResponse.Output = new Output();
                        chatResponse.InstanceId = input.InstanceId;
                        chatResponse.Output.InstanceId = input.InstanceId;
                        chatResponse.Output.MessageId = ++input.MessageId;
                        chatResponse.Output.Content = Constants.Chats.Message.RetryFailed;
                        chatResponse.Output.Role = Role.Assistant;
                        chatResponse.Output.LastUpdatedTime = DateTime.UtcNow;
                        SaveResponseToRedis(key, Chat.ConvertOutputToMessage(chatResponse), "Error", statusCode);
                        UpdateChatInSQL(chatId, Chat.ConvertOutputToMessage(chatResponse), string.Empty, null, "Error", statusCode);
                        return new ContentResult
                        {
                            Content = JsonConvert.SerializeObject(chatResponse.Output),
                            ContentType = Static.Constants.ContentType.TextPlain,
                            StatusCode = statusCode
                        };
                    }
                }
                return response;

            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetStatus - {e.Message}", nameof(GetStatus), exception: e);
                throw;
            }
        }

        public async Task<bool> AppendChatToRedisCache(string key, Chat.Message message)
        {
            try
            {
                var result = await GetDataFromCacheOrDatabase(key);
                Log(AppLogLevel.Trace, $@"Chat | AppendChat | Redis Response : {result}", nameof(AppendChatToRedisCache));
                if (!result.IsNullOrEmpty())
                {
                    Log(AppLogLevel.Trace, $@"Chat | AppendChat | Cached response received : {key}", nameof(AppendChatToRedisCache));
                    List<Chat.Message> chat = JsonConvert.DeserializeObject<List<Chat.Message>>(result);
                    chat.Add(message);

                    await _sessionClient.SetRedisCache(key, JsonConvert.SerializeObject(chat));

                    Log(AppLogLevel.Trace, $@"Chat | AppendChat | Appended response to cached response : {key} {JsonConvert.SerializeObject(chat)}");
                }
                else
                {
                    Log(AppLogLevel.Trace, "Chat | SaveChat | Cached response not found");
                    var messages = new List<Chat.Message> { message };
                    await _sessionClient.SetRedisCache(key, JsonConvert.SerializeObject(messages));
                    Log(AppLogLevel.Trace, $@"Chat | SaveChat | Cached new response : {key} {JsonConvert.SerializeObject(message)}");
                }
                return true;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $@"Chat | SaveChat | Cached response not found {e.StackTrace}", exception: e);
                return false;
            }
        }

        private async Task SaveQuestion(Chat.Input input, Models.Chat.OrchestratorStatus orchestratorStatus)
        {
            try
            {
                //Save Question to Redis
                string key = string.Format(Constants.Redis.Keys.MessageKey, userId.ToString(), input.ChatId);
                //Add question to redis cache
                var message = new Chat.Message
                {
                    MessageId = input.MessageId,
                    InstanceId = orchestratorStatus.Id,
                    Content = input.Question,
                    Role = Role.User,
                    Sources = input.Sources,
                    InputSources = input.InputSources,
                    CreatedTime = DateTime.UtcNow,
                    LastUpdatedTime = DateTime.UtcNow,
                    Context = input.Context,
                };
                Log(AppLogLevel.Trace, $@"Chat | SaveQuestionToRedis | Saving Question to Redis");
                bool isSaved = await AppendChatToRedisCache(key, message);
                List<Chat.Message> messages = new List<Chat.Message> { message };
                if (isSaved)
                {
                    var responseMessage = new Chat.Message
                    {
                        MessageId = message.MessageId + 1,
                        InstanceId = orchestratorStatus.Id,
                        Content = Constants.Chats.Message.RetryPending,
                        Role = Role.Assistant,
                        InputSources = message.InputSources,
                        CreatedTime = DateTime.UtcNow,
                        LastUpdatedTime = DateTime.UtcNow,
                        Status = RunTimeStatus.Pending,
                    };

                    //Add placeholder for response
                    Log(AppLogLevel.Trace, $@"Chat | SaveQuestionToRedis | Add placeholder for response as pending");
                    isSaved = await AppendChatToRedisCache(key, responseMessage);
                    messages.Add(responseMessage);
                }
                SaveChatInSQL(input.ChatId, messages);
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $@"Chat | SaveQuestionToRedis | Error in saving question to Redis {e.StackTrace}", exception: e);
            }
        }

        public async void SaveResponseToRedis(string key, Chat.Message message, string status, int statusCode)
        {
            try
            {
                string jsonString = await GetDataFromCacheOrDatabase(key);
                Log(AppLogLevel.Trace, $@"Chat | SaveResponseToRedis | Current cached message {jsonString}");

                if (!jsonString.IsNullOrEmpty())
                {
                    var conversation = JsonConvert.DeserializeObject<List<Chat.Message>>(jsonString);
                    //find message in conversation.content where messageid = message.id
                    var response = conversation.Find(x => x.MessageId == message.MessageId && x.InstanceId == message.InstanceId);
                    if (response != null)
                    {
                        response.Content = message.Content;
                        response.Role = message.Role;
                        response.Sources = message.Sources;
                        response.InputSources = message.InputSources;
                        response.CreatedTime = DateTime.UtcNow;
                        response.Context = message.Context;
                        response.Response = message.Response;
                        response.LastUpdatedTime = message.LastUpdatedTime;
                        response.CreatedTime = message.CreatedTime;
                        response.Status = status;
                        response.StatusCode = statusCode;
                        response.InstanceId = message.InstanceId;
                        response.FollowUpSuggestions = message.FollowUpSuggestions;
                        Log(AppLogLevel.Trace, $@"Chat | SaveResponseToRedis | Updated cached message {JsonConvert.SerializeObject(conversation)}");
                        await _sessionClient.SetRedisCache(key, JsonConvert.SerializeObject(conversation));
                    }
                }
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $@"Chat | SaveResponseToRedis | Error in saving response to Redis {e.StackTrace}", exception: e);
            }
        }

        private void SaveChatInSQL(string chatId, List<Chat.Message> messages)
        {
            List<Data.Models.ChatHistory> inputMessages = new List<Data.Models.ChatHistory>();
            foreach (var message in messages)
            {
                Data.Models.ChatHistory history = Mapper.ChatHistory.CreateInsertModelForUser(chatId, message, userId.ToString());
                inputMessages.Add(history);
            }
            fireForgetRepositoryHandler.Execute(async newDBContext =>
            {
                newDBContext.ChatHistorys.AddRange(inputMessages);
                await newDBContext.SaveChangesAsync();
            });
        }

        private void UpdateChatInSQL(string chatId, Chat.Message message, string? content, TimeSpan? timeEllapsed, string status, int statusCode)
        {
            fireForgetRepositoryHandler.Execute(async newDBContext =>
            {
                Log(AppLogLevel.Trace, $"Chat | UpdateChatInSQL chat id-{chatId} message id -{message.MessageId}");
                var dbFeedback = newDBContext.ChatHistorys.FirstOrDefault(f => f.InstanceId == message.InstanceId &&
                                                                          f.MessageId == message.MessageId && f.ChatId == chatId);
                Mapper.ChatHistory.CreateUpdateModel(message, dbFeedback, content, timeEllapsed, status, statusCode);
                await newDBContext.SaveChangesAsync();
            });
        }

        public async Task PostFeedback(Chat.MessageFeedback feedback)
        {
            try
            {
                await UpdateChatFeedbackInRedis(feedback);
                await UpdateChatFeedbackInSql(feedback);
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, "Exception occurred" + e.Message, exception: e);
                throw;
            }
        }

        public async Task PostCopilotFeedback(Chat.CopilotFeedback feedback)
        {
            try
            {
                Log(AppLogLevel.Trace, $"Adding Copilot feedback for user {userId}");
                Data.Models.CopilotFeedback modelFeedback = Mapper.CopilotFeedback.CreateInsertModel(feedback, userId.ToString());
                context.CopilotFeedbacks.Add(modelFeedback);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, "Exception occurred" + e.Message, exception: e);
                throw;
            }
        }

        /// <summary>
        /// Retrieves an export file for the specified chat conversation.
        /// </summary>
        /// <param name="chatId">The ID of the chat conversation.</param>
        /// <param name="messageId">The ID of the last message in the chat conversation to include in the export file. Set to 0 to include all messages.</param>
        /// <param name="fileType">The type of export file to generate (e.g., "pdf", "ppt", "docx").</param>
        /// <returns>The export file as a FileContentResult.</returns>
        public async Task<FileContentResult> GetExportFile(string chatId, int messageId, string fileType)
        {
            try
            {
                if (!IsSupportedFileType(fileType))
                {
                    throw new ArgumentException("Unsupported file type.");
                }

                var userEmail = userId?.ToString();
                string key = string.Format(Constants.Redis.Keys.MessageKey, userEmail, chatId);
                Task<PortalUser> portalUser = _portalClient.GetUserDetails(Convert.ToBase64String(Encoding.UTF8.GetBytes(userEmail)));
                Task<string> cachedValue = GetDataFromCacheOrDatabase(key);
                await Task.WhenAll(portalUser, cachedValue);
                Log(AppLogLevel.Information, "Fetched content from Redis cache", nameof(GetExportFile));
                var messages = JsonConvert.DeserializeObject<List<MessageHistory>>(cachedValue.Result);
                messages = messages?.Where(t => messageId == 0 || t.messageId <= messageId).ToList();
                messages.ForEach(message => message.content = Util.ConvertMarkupToHtml(message.content));
                var base64Image = portalUser.Result?.Data?.FirstOrDefault()?.Photo;
                var userImage = string.IsNullOrWhiteSpace(base64Image) ? GetDefaultProfilePicture() : GetUserProfilePicture(base64Image);
                var html = messages != null ? CreateHtmlFromJson(messages, userImage) : string.Empty;
                Log(AppLogLevel.Information, "Genereated HTML content from json", nameof(GetExportFile));
                return await _exportFileClient.GetExportFile(html, fileType);
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Error in GetExportFile - {ex.Message}", nameof(GetExportFile), exception: ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the user's profile picture from a base64 encoded image.
        /// If the conversion to a circular image fails, the default profile picture is returned.
        /// </summary>
        /// <param name="base64Image">The base64 encoded image.</param>
        /// <returns>The user's profile picture as a data URI.</returns>
        private string GetUserProfilePicture(string base64Image)
        {
            try
            {
                Log(AppLogLevel.Information, "Converting user profile picture to a circular image", nameof(GetUserProfilePicture));
                var profilePic = Util.ConvertToCircle(base64Image);
                Log(AppLogLevel.Information, "User profile picture converted to a circular image", nameof(GetUserProfilePicture));
                return $"data:image/png;base64,{profilePic}";
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Error in GetUserProfilePicture - {ex.Message}", nameof(GetUserProfilePicture), exception: ex);
                return GetDefaultProfilePicture();
            }
        }

        /// <summary>
        /// Retrieves the default profile picture.
        /// </summary>
        /// <returns>The path to the default profile picture.</returns>
        private string GetDefaultProfilePicture()
        {
            return "{ImagePath}circle-user-solid.jpg";
        }

        /// <summary>
        /// Checks if the given file type is supported for export.
        /// </summary>
        /// <param name="fileType">The file type to check.</param>
        /// <returns>True if the file type is supported, false otherwise.</returns>
        private static bool IsSupportedFileType(string fileType)
        {
            var supportedExportFileTypes = new List<string> { Constants.SupportedExportFileTypes.Pdf, Constants.SupportedExportFileTypes.Word};
            return supportedExportFileTypes.Contains(fileType, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Creates an HTML string from a list of MessageHistory objects in JSON format.
        /// </summary>
        /// <param name="messages">The list of MessageHistory objects.</param>
        /// <returns>An HTML string representing the messages.</returns>
        private string CreateHtmlFromJson(List<MessageHistory> messages, string userImage)
        {
            var htmlBuilder = new StringBuilder();
            GetSourceConfiguration();
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    if (message.role == "user")
                    {
                        htmlBuilder.AppendLine("<div class=\"messages-list__user-message\">");
                        htmlBuilder.AppendLine("<table>");
                        htmlBuilder.AppendLine("<tr>");
                        htmlBuilder.AppendLine("<td style=\"width:25% \">");
                        htmlBuilder.AppendLine("</td>");
                        htmlBuilder.AppendLine("<td>");
                        htmlBuilder.AppendLine("<div class=\"messages-wrapper\">");
                        htmlBuilder.AppendLine($"<span class=\"messages-list__user-message__heading\">You</span>");
                        htmlBuilder.AppendLine("<div class=\"messages-list__user-message__content\">");
                        htmlBuilder.AppendLine("<table class=\"messages-list__user-message__content__text\">");
                        htmlBuilder.AppendLine("<tr><td><p>");
                        htmlBuilder.AppendLine(message.content.Replace("\n", "<br>"));
                        htmlBuilder.AppendLine("</p></td></tr>");
                        htmlBuilder.AppendLine("</table>");
                        htmlBuilder.AppendLine("</div>");
                        htmlBuilder.AppendLine("</div>");
                        htmlBuilder.AppendLine("</td>");
                        htmlBuilder.AppendLine("<td style=\"vertical-align:top;width: 9%\">");
                        htmlBuilder.AppendLine($"<img src=\"{userImage}\" class=\"user-profile-image\">");
                        htmlBuilder.AppendLine("</td>");
                        htmlBuilder.AppendLine("</tr>");
                        htmlBuilder.AppendLine("</table>");
                        htmlBuilder.AppendLine("</div>");
                    }
                    else if (message.role == "assistant")
                    {
                        htmlBuilder.AppendLine("<div class=\"messages-list__assistant-message\">");
                        htmlBuilder.AppendLine("<table>");
                        htmlBuilder.AppendLine("<tr>");
                        htmlBuilder.AppendLine("<td style=\"vertical-align:top;width:7%;\">");
                        htmlBuilder.AppendLine("<img src=\"{ImagePath}ce_assistant_icon.jpg\" alt=\"CE Assistant Icon\">");
                        htmlBuilder.AppendLine("</td>");
                        htmlBuilder.AppendLine("<td style=\"width:80%;\">");
                        htmlBuilder.AppendLine("<div class=\"messages-wrapper\">");
                        htmlBuilder.AppendLine("<table class=\"messages-list__assistant-message__heading-wrapper\">");
                        htmlBuilder.AppendLine("<tr>");
                        htmlBuilder.AppendLine("<td class=\"messages-list__assistant-message__heading\">Capital Edge Assistant</td>");
                        var sources = string.Empty;
                        message.content = ReplaceSourceKeyInContent(message.content);
                        if (message.inputSources != null && message.inputSources.Any())
                        {
                            foreach (var source in message.inputSources)
                            {
                                if (string.IsNullOrWhiteSpace(source))
                                {
                                    continue;
                                }

                                string sourceDisplayName = GetSourceDisplayName(source);

                                if (sources != string.Empty)
                                {
                                    sources = sources + " | " + sourceDisplayName;
                                }
                                else
                                {
                                    sources = sourceDisplayName;
                                }
                            }
                        }
                        htmlBuilder.AppendLine("<td class=\"messages-list__assistant-message__sources\">" + sources + "</td>");
                        htmlBuilder.AppendLine("</tr>");
                        htmlBuilder.AppendLine("</table>");
                        htmlBuilder.AppendLine("<div class=\"messages-list__assistant-message__content\">");
                        htmlBuilder.AppendLine("<table class=\"messages-list__assistant-message__content__text\">");
                        htmlBuilder.AppendLine("<tr><td><p>");
                        htmlBuilder.AppendLine(message.content.Replace("\n", "<br>"));
                        htmlBuilder.AppendLine("</p></td></tr>");
                        htmlBuilder.AppendLine("</table>");
                        htmlBuilder.AppendLine("</div>");
                        htmlBuilder.AppendLine("</div>");
                        htmlBuilder.AppendLine("</td>");
                        htmlBuilder.AppendLine("<td></td>");
                        htmlBuilder.AppendLine("</tr>");
                        htmlBuilder.AppendLine("<tr><td></td><td class=\"reference_td\">");

                        if (message?.response != null && message.response.Any())
                        {
                            var supportedReferences = new List<string> {
                                Constants.ReferenceDisplayItems.FileName,
                                Constants.ReferenceDisplayItems.Table,
                                Constants.ReferenceDisplayItems.Documents,
                            };
                            var isReferenceTxtAdded = false;
                            foreach (var response in message.response)
                            {
                                if (response.citingSources != null)
                                {
                                    foreach (var citingSource in response.citingSources)
                                    {
                                        if (citingSource.sourceType != null
                                            && supportedReferences.Any(type => string.Equals(citingSource.sourceType, type, StringComparison.OrdinalIgnoreCase))
                                            && citingSource.sourceValue != null)
                                        {
                                            foreach (var source in citingSource.sourceValue)
                                            {
                                                var sourceName = Util.GetDocumentName(citingSource.sourceName, source);

                                                if (string.IsNullOrWhiteSpace(sourceName))
                                                {
                                                    continue;
                                                }

                                                if (!isReferenceTxtAdded)
                                                {
                                                    isReferenceTxtAdded = true;
                                                    htmlBuilder.AppendLine("<span class=\"references__container__text\">References</span>");
                                                    htmlBuilder.AppendLine("<table class=\"reference_values\">");
                                                }
                                                htmlBuilder.AppendLine("<tr><td class=\"reference_bullet_point\"><img src=\"{ImagePath}square-solid.jpg\"></td><td class=\"references__container__pdf_reference\">");
                                                htmlBuilder.AppendLine(sourceName);
                                                htmlBuilder.AppendLine("</td></tr>");
                                            }
                                        }
                                    }
                                }
                            }

                            if (isReferenceTxtAdded)
                            {
                                htmlBuilder.AppendLine("</table>");
                            }
                        }
                        htmlBuilder.AppendLine("</td><td></td></tr>");
                        htmlBuilder.AppendLine("</table>");
                        htmlBuilder.AppendLine("</div>");
                    }
                }
            }

            return htmlBuilder.ToString();
        }

        string ReplaceSourceKeyInContent(string content)
        {
            foreach (var source in Constants.Chats.Sources)
            {
                string replaceStr = String.Format(Constants.Chats.SourceKeyFormat, source.Key);
                int index = content.IndexOf(replaceStr);
                if (index >= 0)
                {
                    content = content.Replace(replaceStr, GetSourceDisplayName(source.Key));
                }
            }
            return content;
        }
        void GetSourceConfiguration()
        {
            var config = context.CopilotConfigurations.AsNoTracking().FirstOrDefault(col => col.Key == "SOURCE_CONFIGS");
            if (config != null)
            {
                sourceConfigurations = JsonConvert.DeserializeObject<List<SourceConfiguration>>(config.Value);
            }
        }
        string GetSourceDisplayName(string source)
        {
            return sourceConfigurations.Where(config => config.Key == source).Select(config => config.DisplayName).FirstOrDefault();
        }
        private async Task UpdateChatFeedbackInSql(Chat.MessageFeedback feedback)
        {
            Log(AppLogLevel.Trace, $"Chat | UpdateChatFeedbackInSql chat id-{feedback.ChatId} message id -{feedback.MessageId} instance id - {feedback.InstanceId}");
            var dbFeedback = await context.MessageFeedbacks
                                    .FirstOrDefaultAsync(f => f.MessageId == feedback.MessageId && f.ChatId == feedback.ChatId && (feedback.InstanceId==null || f.InstanceId == feedback.InstanceId));
            if (dbFeedback != null)
            {
                Log(AppLogLevel.Trace, $"updating feedback for user {userId} for message id {feedback.MessageId} chat id {feedback.ChatId} instance id - {feedback.InstanceId}");
                Mapper.MessageFeedback.CreateUpdateModel(feedback, dbFeedback);
            }
            else
            {
                Log(AppLogLevel.Trace, $"Adding feedback for user {userId} for message id {feedback.MessageId} chat id {feedback.ChatId} instance id - {feedback.InstanceId}");
                Data.Models.MessageFeedback modelFeedback = Mapper.MessageFeedback.CreateInsertModel(feedback, userId.ToString());
                context.MessageFeedbacks.Add(modelFeedback);
            }
            await context.SaveChangesAsync();
        }

        private async Task UpdateChatFeedbackInRedis(Chat.MessageFeedback feedback)
        {
            try
            {
                Log(AppLogLevel.Trace, $"Chat | UpdateChatFeedbackInRedis chat id-{feedback.ChatId} message id -{feedback.MessageId} instanceid- {feedback.InstanceId}");
                string key = string.Format(Constants.Redis.Keys.MessageKey, userId.ToString(), feedback.ChatId);
                var result = await GetDataFromCacheOrDatabase(key);
                Log(AppLogLevel.Trace, $@"Chat | Update Chat | Redis Response : {result}");
                List<Chat.Message> chat = JsonConvert.DeserializeObject<List<Chat.Message>>(result);
                foreach (var message in chat)
                {
                    if (message.MessageId == feedback.MessageId && (feedback.InstanceId == null || message.InstanceId == feedback.InstanceId))
                    {
                        if (message.Context == null) message.Context = new Context();
                        message.Context.IsMessageLiked = feedback.IsLiked;
                        message.Context.Consent = feedback.Consent;
                    }
                }

                await _sessionClient.SetRedisCache(key, JsonConvert.SerializeObject(chat));
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, "Error updating feedback in redis " + ex.Message, exception: ex);
            }
        }

        private string GenerateContentFromResponse(List<Chat.Response> response, Chat.Input input)
        {
            List<string>? selectedSources = input.InputSources;
            var content = new StringBuilder();

            bool excludeSummary = CheckExclusion(input, response);
            bool hasSuggestion = CheckSuggestion(input, response);

            if (selectedSources?.Count == 1)
            {
                string contentToDisplay = GetDisplayContentFromResponsesByStatus(response, selectedSources[0]);
                content.AppendLine(contentToDisplay);
            }
            else if (selectedSources?.Count == 2)
            {
                AppendMultipleSourcesContent(response, selectedSources, content, excludeSummary, hasSuggestion);
            }

            return content.ToString();
        }

        private bool CheckExclusion(Chat.Input input, List<Chat.Response> response)
        {
            return input?.InputSources?.Count == 1 || !string.IsNullOrWhiteSpace(input?.Context?.Suggestion?.Id);
        }

        private bool CheckSuggestion(Chat.Input input, List<Chat.Response> response)
        {
            if (!string.IsNullOrEmpty(input?.Context?.Suggestion?.Id))
            {
                return true;
            }
            return false;
        }

        private string GetDisplayContentFromResponsesByStatus(List<Chat.Response> response, string source)
        {
            var status = response.Find(x => x.SourceName.Equals(source))?.status.ToString();
            if (status == StatusCodes.Status200OK.ToString())
            {
                return response.Find(x => x.SourceName.Equals(source))?.content.ToString() ?? "";
            }
            else if (status == StatusCodes.Status401Unauthorized.ToString() || status == StatusCodes.Status500InternalServerError.ToString())
            {
                return Constants.Chats.Message.SourceUnavailable;
            }
            else if (status == StatusCodes.Status400BadRequest.ToString())
            {
                var contentResponse = response.Find(x => x.SourceName.Equals(source))?.content.ToString();
                if (contentResponse != null && contentResponse.ToLower().Contains(Constants.SQLErrorText.ToLower()))
                {
                    return Constants.Chats.Message.ErrorOnWrongSQL;
                }
            }
            return Constants.Chats.Message.GeneralError;
        }

        private void AppendMultipleSourcesContent(List<Chat.Response> response, List<string> sources, StringBuilder content, bool excludeSummary, bool hasSuggestion)
        {
            var failedResponses = response.FindAll(x => sources.Contains(x.SourceName)
                                                    && x.status != StatusCodes.Status200OK.ToString());
            if (failedResponses.Count > 0)
            {
                AppendFailedResponseContent(response, sources, content, hasSuggestion);
            }
            else if (failedResponses.Count == 0)
            {
                AppendSuccessfulResponseContent(response, sources, content, excludeSummary, hasSuggestion);
            }
        }

        private void AppendFailedResponseContent(List<Chat.Response> response, List<string> sources, StringBuilder content, bool hasSuggestion)
        {
            foreach (var source in sources)
            {
                var responseItem = response.Find(x => x.SourceName.Equals(source));
                if (responseItem != null)
                {
                    content.AppendLine("<b>"+ String.Format(Constants.Chats.SourceKeyFormat, source)  + "</b>");
                    string contentToDisplay = GetDisplayContentFromResponsesByStatus(response, source);
                    content.AppendLine(contentToDisplay);
                    content.Append(Constants.NewLine);
                }
            }
        }

        private void AppendSuccessfulResponseContent(List<Chat.Response> responses, List<string> sources, StringBuilder content, bool excludeSummary, bool hasSuggestion)
        {
            if (!excludeSummary)
            {
                content.AppendLine("<b>" + Constants.Chats.Summary + "</b>");
                content.AppendLine(responses?.Find(x => x.SourceName.Equals(Constants.Chats.Summary.ToLower()))?.content);
                content.Append(Constants.NewLine);
            }

            var successfulResponse = responses.FindAll(x => sources.Exists(y => y.Equals(x.SourceName)));

            foreach (var response in successfulResponse)
            {
                content.AppendLine("<b>" + String.Format(Constants.Chats.SourceKeyFormat, response.SourceName) + "</b>");
                content.AppendLine(response.content);
                content.Append(Constants.NewLine);
            }

            if (hasSuggestion)
            {
                var sourceNotInResponse = sources.FindLast(x => !responses.Exists(y => y.SourceName.Equals(x)));
                if (sourceNotInResponse != null)
                {
                    content.AppendLine("<b>" + String.Format(Constants.Chats.SourceKeyFormat, sourceNotInResponse) + "</b>");
                    content.AppendLine(Constants.Chats.Message.NotApplicable);
                    content.Append(Constants.NewLine);
                }
            }
        }

        /// <summary>
        /// Retrieves data from the cache or the database based on the provided key.
        /// </summary>
        /// <param name="key">The key used to retrieve the data.</param>
        /// <returns>The cached value or the data retrieved from the database.</returns>
        public async Task<string> GetDataFromCacheOrDatabase(string key)
        {
            var cachedValue = await _sessionClient.GetRedisCache(key);

            if (configuration.GetValue<bool>(ConfigMap.IS_REDIS_UPDATE_FROM_DB_ENABLED) && string.IsNullOrWhiteSpace(cachedValue))
            {
                var keys = ExtractIdsFromKey(key);

                if (keys != null)
                {
                    var (userId, chatId) = keys.Value;
                    var messages = await GetChatHistoryFromSQL(userId.ToString(), chatId.ToString());

                    if (messages != null && messages.Any())
                    {
                        cachedValue = JsonConvert.SerializeObject(messages);
                        await _sessionClient.SetRedisCache(key, cachedValue);
                    }
                }
            }

            return cachedValue;
        }

        /// <summary>
        /// Retrieves the chat history from the SQL database for a specific user and chat ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="chatId">The ID of the chat.</param>
        /// <returns>A list of messages representing the chat history.</returns>
        private async Task<List<Message>> GetChatHistoryFromSQL(string userId, string chatId)
        {
            var messages = new List<Message>();

            try
            {
                List<Data.Models.ChatHistory> chatHistories = await context.ChatHistorys
                                         .Where(c => c.ChatId == chatId && c.UserId == userId)
                                         .OrderBy(t => t.CreatedAt)
                                         .ToListAsync();

                foreach (var chatHistory in chatHistories)
                {
                    if (!string.IsNullOrWhiteSpace(chatHistory.AdditionalInfo))
                    {
                        var chatResponse = JsonConvert.DeserializeObject<Chat.ResponseObject>(chatHistory.AdditionalInfo);

                        if (chatResponse?.Output != null)
                        {
                            var message = ConvertOutputToMessage(chatResponse);
                            message.Content = chatHistory.Message;
                            message.Status = chatHistory.Status;
                            message.StatusCode = chatHistory.StatusCode;
                            messages.Add(message);
                            continue;
                        }
                    }

                    messages.Add(new Message
                    {
                        Content = chatHistory.Message,
                        CreatedTime = chatHistory.CreatedAt,
                        InstanceId = chatHistory.InstanceId,
                        LastUpdatedTime = chatHistory.UpdatedAt,
                        MessageId = chatHistory.MessageId,
                        Role = chatHistory.UserRole,
                        Status = chatHistory.Status,
                        StatusCode = chatHistory.StatusCode,
                    });
                }
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, "Exception occurred" + ex.Message, nameof(GetChatHistoryFromSQL), exception: ex);
            }

            return messages;
        }

        /// <summary>
        /// Extracts the user ID and chat ID from a given key.
        /// </summary>
        /// <param name="key">The key to extract the IDs from.</param>
        /// <returns>A tuple containing the user ID and chat ID if the extraction is successful, otherwise null.</returns>
        private static (string userId, string chatId)? ExtractIdsFromKey(string key)
        {
            string pattern = @"user:(.*):chatid:(.*)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(key);

            if (match.Success)
            {
                string userId = match.Groups[1].Value;
                string chatId = match.Groups[2].Value;

                return (userId, chatId);
            }

            return null;
        }

        public async Task<List<ActivePOApp>> GetAppsfromCacheorPortal()
        {
            try
            {
                List<ActivePOApp> apps = new List<ActivePOApp>();
                string key = string.Format(Constants.Redis.Keys.AppsKey, userId.ToString());
                var value = await _sessionClient.GetRedisCache(key);
                if (value != null)
                {
                    Log(AppLogLevel.Trace, $"Chat | GetAppsfromCacheorPortal | Apps found in Redis: {value}");
                    apps = JsonConvert.DeserializeObject<List<ActivePOApp>>(value);
                }
                else
                {
                    Log(AppLogLevel.Trace, $"Chat | GetAppsfromCacheorPortal | Apps not found in Redis");
                    apps = await _portalClient.GetApps(idpToken);
                    Log(AppLogLevel.Trace, $"Chat | GetAppsfromCacheorPortal | Apps from Portal : {JsonConvert.SerializeObject(apps).ToString()}");
                    await _sessionClient.SetRedisCache(key, JsonConvert.SerializeObject(apps));
                }
                return apps;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in GetAppsfromCacheorPortal - {e.Message}", nameof(GetAppsfromCacheorPortal), exception: e);
                throw;
            }
        }
    }
}
