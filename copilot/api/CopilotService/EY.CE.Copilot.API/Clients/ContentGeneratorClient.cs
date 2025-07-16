using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Mapper;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using EY.CE.Copilot.Data.Contexts;
using EY.CE.Copilot.Data.Models;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using System.Net;

namespace EY.CE.Copilot.API.Clients
{
    public class ContentGeneratorClient: BaseClass, IContentGeneratorClient
    {
        private readonly CopilotContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISessionClient _sessionClient;
        private readonly string _workspaceName;
        private readonly IPortalClient _portalClient;
        private readonly string UserEmail;
        private readonly HttpContext httpContext;
        public ContentGeneratorClient(CopilotContext context, ISessionClient sessionClient, IHttpContextAccessor httpContextAccessor, IPortalClient portalClient,  IHttpClientFactory httpClientFactory, IConfiguration configuration, IAppLoggerService logger) : base(logger, nameof(ContentGeneratorClient))
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _sessionClient = sessionClient;
            _workspaceName = configuration[ConfigMap.WORKSPACE_NAME];
            _portalClient = portalClient;
            httpContext = httpContextAccessor.HttpContext;
            if (httpContext.Items.TryGetValue(Constants.UserMail, out object? mail))
                UserEmail = mail.ToString();
        }

        public async Task<IEnumerable<ContentGeneratorQuery>> GetDataByFilter(string app, string generatorType)
        {
            try
            {
                Log(AppLogLevel.Trace, "Getting queries for content generator", nameof(GetDataByFilter));
                IQueryable<ContentGeneratorQuery> dbGlossary;

                if (!string.IsNullOrWhiteSpace(app) && !string.IsNullOrWhiteSpace(generatorType))
                {
                    dbGlossary = _context.ContentGeneratorQueries.Where(query => query.APP == app && query.GeneratorType == generatorType);
                }
                else if (!string.IsNullOrWhiteSpace(app))
                {
                    dbGlossary = _context.ContentGeneratorQueries.Where(query => query.APP == app);
                }
                else if (!string.IsNullOrWhiteSpace(generatorType))
                {
                    dbGlossary = _context.ContentGeneratorQueries.Where(query => query.GeneratorType == generatorType);
                }
                else
                {
                    dbGlossary = _context.ContentGeneratorQueries;
                }
                return dbGlossary;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetAll - {e.Message}", nameof(GetDataByFilter), exception: e);
                throw;
            }
        }

        public async Task<object> GetDataFromRedis(string instanceId, ContentGenerator input)
        {
            var redisOutput = await _sessionClient.GetRedisCache(instanceId);
            Log(AppLogLevel.Information, $"Redis output for instance id {instanceId} - {redisOutput}");

            if (string.IsNullOrWhiteSpace(redisOutput))
            return null;

            if (input.appType == AppType.WorkplanGenerator)
            {
                return await ProcessWorkplan(instanceId, input, redisOutput);
            }
            else if (input.appType == AppType.StatusReportGenerator)
            {
                return await ProcessProjectStatus(instanceId, redisOutput);
            }
            else if (input.appType == AppType.ProjectCharterGenerator)
            {
                return await ProcessProjectCharter(instanceId, redisOutput);
            }
            else if (input.appType == AppType.OpModelGenerator)
            {
                return await ProcessOPModel(instanceId, redisOutput);
            }
            else if (input.appType == AppType.TSAGenerator)
            {
                return await ProcessTSA(instanceId, redisOutput, input.ProjectTeamsRemoveList);
            }
            else
            {
                return null;
            }
        }

        private async Task<object> ProcessProjectCharter(string instanceId, string redisOutput)
        {
            var projectCharter = JsonConvert.DeserializeObject<ProjectCharter>(redisOutput);

            try
            {
                if (!projectCharter.runtimeStatus.ToLower().Equals(RunTimeStatus.InProgress))
                {
                    var content = CreateGeneratorActivityRecord(instanceId, redisOutput, projectCharter.runtimeStatus.ToLower(), projectCharter.name);
                    await UpdateContentInDB(content);
                }
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Error in method ProcessProjectCharter | UpdateContent in database failed - {ex.Message}", nameof(ProcessProjectCharter), exception: ex);
            }

            return projectCharter;
        }


        private async Task<object> ProcessWorkplan(string instanceId, ContentGenerator input, string redisOutput)
        {
            input.Workplan = new Workplan();
            input.Workplan.Output = JsonConvert.DeserializeObject<WorkplanOutputFinal>(redisOutput);

            try
            {
            if (!input.Workplan.Output.runtimeStatus.ToLower().Equals(RunTimeStatus.InProgress))
            {
                var content = CreateGeneratorActivityRecord(instanceId, redisOutput, input.Workplan.Output.runtimeStatus.ToLower());
                await UpdateContentInDB(content);
            }
            }
            catch (Exception e)
            {
            Log(AppLogLevel.Error, $"Error in method ProcessWorkplan UpdateContent in database failed - {e.Message}", nameof(ProcessWorkplan), exception: e);
            }

            if (input.Workplan.Output.output is not null)
            input.Workplan.Output.output.response = input.Workplan.FilterOutputByTeams(input.ProjectTeamsRemoveList);

            return input.Workplan.Output;
        }

        private async Task<object> ProcessProjectStatus(string instanceId, string redisOutput)
        {
            var projectStatusJson = JsonConvert.DeserializeObject<ProjectStatusJson>(redisOutput);
            var projectStatus = MapProjectStatus(projectStatusJson);

            try
            {
            if (!projectStatusJson.runtimeStatus.ToLower().Equals(RunTimeStatus.InProgress))
            {
                var content = CreateGeneratorActivityRecord(instanceId, redisOutput, projectStatusJson.runtimeStatus.ToLower(), projectStatusJson.name);
                await UpdateContentInDB(content);
            }
            }
            catch (Exception ex)
            {
            Log(AppLogLevel.Error, $"Error in method ProcessProjectStatus | UpdateContent in database failed - {ex.Message}", nameof(ProcessProjectStatus), exception: ex);
            }

            return projectStatus;
        }

        private GeneratorActivityRecord CreateGeneratorActivityRecord(string instanceId, string redisOutput, string runtimeStatus, string type = "")
        {
            return new GeneratorActivityRecord
            {
                InstanceId = instanceId,
                AdditionalInfo = redisOutput,
                Status = runtimeStatus,
                StatusCode = runtimeStatus == RunTimeStatus.Completed ? (int)HttpStatusCode.OK : runtimeStatus == RunTimeStatus.Aborted ? 499 : (int)HttpStatusCode.InternalServerError,
                UserId = UserEmail,
                Type = type
            };
        }

        private ProjectStatus MapProjectStatus(ProjectStatusJson projectStatusJson)
        {
            var projectStatus = new ProjectStatus
            {
            name = projectStatusJson.name,
            runtimeStatus = projectStatusJson.runtimeStatus,
            instanceId = projectStatusJson.instanceId,
            lastUpdatedTime = projectStatusJson.lastUpdatedTime,
            createdTime = projectStatusJson.createdTime,
            output  = new Response()
            };
            if (projectStatusJson.output is not null)
            {
                projectStatus.output.response = new List<object>();
                foreach (var item in projectStatusJson.output.response)
                {
                    string sourceName = item.Value<string>("sourceName");
                    projectStatus.output.response.Add(sourceName switch
                    {
                        "project-status-executive-summary" => item.ToObject<ContentString>(),
                        "project-status-next-steps" or "project-status-accomplishments" => item.ToObject<ContentArray>(),
                        "project-status-overall-status" => item.ToObject<ContentObject>(),
                        _ => null
                    });
                }
            }
            return projectStatus;
        }

        public async Task<HttpStatusCode> TerminateContentGenerator(AppType appType, string instanceId)
        {
            try
            {               
                using var httpClient = _httpClientFactory.CreateClient(Constants.ContentGeneratorHttpClient);
                string endpoint = appType switch
                {
                    AppType.WorkplanGenerator => string.Format(ContentGeneratorEndpoint.TerminateWorkplan, instanceId),
                    AppType.StatusReportGenerator => string.Format(ContentGeneratorEndpoint.TerminateProjectStatus, instanceId),
                    AppType.ProjectCharterGenerator => string.Format(ContentGeneratorEndpoint.TerminateProjectCharter, instanceId),
                    _ => throw new ArgumentException("Invalid AppType", nameof(appType))
                };

                Log(AppLogLevel.Information, $"Terminating content generator request for instance id {instanceId}");
                var response = await httpClient.PostAsync(endpoint,null);
                return response.StatusCode;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method TerminateContentGenerator - {e.Message}", nameof(TerminateContentGenerator), exception: e);
                throw;
            }
        }

        public async Task StartRequest(AsyncOperationRequest request, string instanceId)
        {
            await _portalClient.TriggerWorkflows(request, UserEmail, instanceId);
            try
            {
                var payloadString = Serializer.SerializeToJson(request);
                JObject json = JObject.Parse(payloadString);
                string requestType = json["payload"]["requestType"].ToString();
                GeneratorActivityRecord content = new GeneratorActivityRecord
                {
                    InstanceId = instanceId,
                    AdditionalInfo = string.Empty,
                    Status = RunTimeStatus.InProgress,
                    StatusCode = (int)HttpStatusCode.OK,
                    Type = requestType,
                    UserId = UserEmail
                };
                await SaveContentInDB(content);
            }
            catch (Exception e)
            { 
                Log(AppLogLevel.Error, $"Error in method StartRequest - {e.Message}", nameof(StartRequest), exception: e);
            }            
        }

        public async Task SaveContentInDB(GeneratorActivityRecord content)
        {
            try
            {
                var generatorContent = GeneratorHistoryMapper.CreateInsertModelForUser(content);
                _context.GeneratorHistory.Add(generatorContent);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method SaveContent - {e.Message}", nameof(SaveContentInDB), exception: e);
                throw;
            }
        }

        public async Task UpdateContentInDB(GeneratorActivityRecord content)
        {
            try
            {
                var generatorContent = _context.GeneratorHistory.FirstOrDefault(x => x.InstanceId == content.InstanceId);
                if (generatorContent != null)
                {
                     generatorContent = GeneratorHistoryMapper.CreateUpdateModelForUser(content, generatorContent);
                    _context.GeneratorHistory.Update(generatorContent);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method UpdateContent - {e.Message}", nameof(UpdateContentInDB), exception: e);
                throw;
            }
        }

        private async Task<object> ProcessOPModel(string instanceId, string redisOutput)
        {
            var opModel = JsonConvert.DeserializeObject<OpModel>(redisOutput);

            try
            {
                if (!opModel.runtimeStatus.ToLower().Equals(RunTimeStatus.InProgress))
                {
                    var content = CreateGeneratorActivityRecord(instanceId, redisOutput, opModel.runtimeStatus.ToLower(), opModel.name);
                    await UpdateContentInDB(content);
                }
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Error in method ProcessOPModel | UpdateContent in database failed - {ex.Message}", nameof(ProcessOPModel), exception: ex);
            }
            return opModel;
        }

        private async Task<object> ProcessTSA(string instanceId, string redisOutput, List<string> projectTeamRemoveList)
        {
            var tsa = JsonConvert.DeserializeObject<TSAObject>(redisOutput);
            try
            {
                if (!tsa.runtimeStatus.ToLower().Equals(RunTimeStatus.InProgress))
                {
                    var content = CreateGeneratorActivityRecord(instanceId, redisOutput, tsa.runtimeStatus.ToLower(), tsa.name);
                    await UpdateContentInDB(content);
                }
                Log(AppLogLevel.Information, "Deserializing whole response object");
                var tsaResponse = JsonConvert.DeserializeObject<TSA>(redisOutput);
                List<TSAResponse> filteredOutput = new List<TSAResponse>();
                if (tsaResponse.output != null && tsaResponse.output.response != null)
                {
                    foreach (var item in tsaResponse.output.response)
                    {
                        if (projectTeamRemoveList is null || !projectTeamRemoveList.Contains(item.projectTeam.title))
                        {
                            filteredOutput.Add(item);
                        }
                    }
                    tsaResponse.output.response = filteredOutput;
                    return tsaResponse;
                }
                
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Error in method ProcessTSA | UpdateContent in database failed - {ex.Message}", nameof(ProcessTSA), exception: ex);
                //these properties are not null in case of TSA Generation in which case if there is a deserialization error then return null.
                //these properties are null in case of service scope generation in which case the redis string should be returned without deserialization.s
                if (tsa.input.ceApps != null || tsa.input.eyIP != null)
                    return null;
            }
            return redisOutput;
        }
    }
}

