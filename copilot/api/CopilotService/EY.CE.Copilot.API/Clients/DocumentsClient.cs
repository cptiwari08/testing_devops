using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Extensions;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using EY.SAT.CE.SharePoint.Contracts;
using EY.SAT.CE.SharePoint.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using static EY.CE.Copilot.API.Static.Constants;
using EY.SAT.CE.SharePoint.Models.OData;

namespace EY.CE.Copilot.API.Clients
{
    public class DocumentsClient : BaseClass, IDocumentsClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string UserEmail;
        private readonly string SpUrl;
        private readonly ISessionClient RedisClient;
        private readonly ISharePointService SharePointService;

        public DocumentsClient(IConfiguration configuration, IHttpClientFactory clientFactory, IAppLoggerService logger, IHttpContextAccessor httpContextAccessor, ISessionClient redisClient,
            ISharePointService sharePointService) : base(logger, nameof(DocumentsClient))
        {
            _configuration = configuration;
            _clientFactory = clientFactory;

            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext.Items.TryGetValue(Constants.UserMail, out object? mail))
                UserEmail = mail.ToString();
            if (httpContext.Items.TryGetValue(Constants.CustomClaimTypes.SpUrl, out object? spUrl))
                SpUrl = spUrl.ToString();

            RedisClient = redisClient;
            SharePointService = sharePointService;
        }

        public async Task<Stream> GetDocumentFromHelpAssistant(string documentId, string token)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                string url = $@"{_configuration[Static.ConfigMap.HELP_ASSISTANT_BASE_URL]}{string.Format(Static.HelpAssistantEndpoints.GetDocument, documentId)}";

                client.DefaultRequestHeaders.Add(Constants.CEAuthHeader, Constants.CEAuthHeaderValue);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStreamAsync();
                }
                else
                {
                    throw new Exception($"Failed to make GET request. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
                }
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetDocumentfromHelpAssistant - {e.Message}", nameof(GetDocumentFromHelpAssistant), exception: e);
                throw;
            }
        }

        public async Task<IEnumerable<FileItem>> GetProjectDocs(DocsRequest docsRequest, bool onbehalfOfUser)
        {
            var projectDocsRedisKey = String.Format(Constants.Redis.Keys.ProjectDocs, string.IsNullOrEmpty(UserEmail) ? docsRequest.User : UserEmail, docsRequest.GeneratorType);
            IEnumerable<FileItem> projectDocs = await GetProjectDocsFromRedisCache(projectDocsRedisKey);

            if (projectDocs == null)
            {
                projectDocs = await GetAssistantDocs(docsRequest, onbehalfOfUser);
                if (projectDocs.Count() > 0 && projectDocs.Any(doc => doc.AssistantProcessingStatus == SharePoint.AssistantProcessingStatusReady))
                {
                    string projectDocsKey = string.Format(projectDocsRedisKey, UserEmail);
                    RedisClient.SetRedisCache(projectDocsKey, JsonSerializer.Serialize(projectDocs));
                }    
            }

            return projectDocs;

            async Task<IEnumerable<FileItem>> GetProjectDocsFromRedisCache(string redisKey)
            {
                IEnumerable<FileItem>? projectDocs = null;
                string projectDocsKey = string.Format(redisKey, UserEmail);
                string projectDocsValue = await RedisClient.GetRedisCache(projectDocsKey);
                Console.WriteLine($"Redis response|{projectDocsKey} |projectDocsValue");
                if (!string.IsNullOrWhiteSpace(projectDocsValue))
                {
                    projectDocs = JsonSerializer.Deserialize<IEnumerable<FileItem>>(projectDocsValue);
                }

                return projectDocs;
            }
        }
           
        public async Task<IEnumerable<FileItem>> GetAssistantDocs(DocsRequest request, bool onbehalfOfUser = true)
        {
            IEnumerable<FileItem> projectDocs;
            if (onbehalfOfUser)
            {
                if (string.IsNullOrWhiteSpace(SpUrl))
                {
                    return Enumerable.Empty<FileItem>();
                }

                SharePointService.SetSiteConfigs(SpUrl, new Uri(SpUrl).GetLeftPart(UriPartial.Authority));
            }
            else
            {
                await SharePointService.LoadSiteConfigs();
            }

            var odataFilter = GetOdataFilter(request);
            ODataOptions oDataOptions = new ODataOptions { Top = "5000", Filter = odataFilter, Select = Constants.SharePoint.DocumentsODataSelect, Expand = Constants.SharePoint.DocumentsODataExpand };
            HttpResponseMessage? result = await SharePointService.GetAllDocuments(oDataOptions, false, onbehalfOfUser);

            if (result == null)
            {
                return Enumerable.Empty<FileItem>();
            }

            var spFileListItemsResponse = result.ToResponseModel<ItemsResponse<SpFileListItem>>();
            if (spFileListItemsResponse != null && spFileListItemsResponse.Value.Count != 0)
            {
                projectDocs = spFileListItemsResponse.Value.Select(item => new FileItem
                {
                    ID = item.GUID,
                    Name = item.File.Name,
                    Size = item.File.Length,
                    Path = item.File.ServerRelativeUrl,
                    LinkingUri = item.File.LinkingUri,
                    EmbedUri = item.ServerRedirectedEmbedUri,
                    VisibleToAssistant = item.VisibleToAssistant,
                    AssistantProcessingStatus = item.AssistantProcessingStatus,
                    AssistantProcessingStatusMessage = item.AssistantProcessingStatusMessage,
                    AuthorId = item.Author.EMail,
                    EditorId = item.Editor.EMail,
                    Created = item.Created,
                    Modified = item.Modified
                });
            }
            else
            {
                projectDocs = Enumerable.Empty<FileItem>();
            }

            return projectDocs;
        }
        public string GetOdataFilter(DocsRequest request)
        {
            var odataFilter = string.Empty;
            odataFilter += SharePoint.DocumentsODataFilterAssistantProcessingStatus;
            
            if(string.IsNullOrEmpty(request.Filter) && (request.GeneratorType==GeneratorType.ChatRequest))
                odataFilter += SharePoint.ODataAnd + SharePoint.DocumentsODataFilterGeneratorTypeChatRequest;
            else if (string.IsNullOrEmpty(request.Filter) && (request.GeneratorType == GeneratorType.WorkplanRequest))
                odataFilter += SharePoint.ODataAnd + SharePoint.DocumentsODataFilterGeneratorTypeWorkplanRequest;
            else if (!string.IsNullOrEmpty(request.Filter))
                odataFilter += SharePoint.ODataAnd + request.Filter;

            return odataFilter;
        }
    }
}
