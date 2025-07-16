namespace EY.CE.Copilot.API.Static
{
    public class Constants
    {
        public const string Authorization = "Authorization";
        public const string CEAuthHeader = "ce-auth";
        public const string CEAuthHeaderValue = null;
        public const string CorrelationId = "correlation-Id";
        public const string ContentGeneratorHttpClient = "ContentGeneratorHttpClient";
        public const string UserMail = "emailaddress";
        public const string UserAgent = "User-Agent";
        public const string ApplicationPlatformName = "Analytics";
        public const string ModuleName = "CoPilot";
        public const string SharePointKey = "SHAREPOINT";
        public const string SSPHttpClient = "SspHttpClient";
        public const string Bearer = "Bearer";
        public const string UserTypeAllowed = "Internal";
        public const string IsAssistantEnabled = "IS_ASSISTANT_ENABLED";
        public const string PortalConfigurationPrefix = "PORTAL:";
        public const string NewLine = "\n";
        public const string DevelopmentEnvironment = "development";
        public const string ProgramOfficeHttpClient = "ProgramOfficeHttpClient";
        public const string ProgramOfficeBlobContainerPrefix = "ce4-";
        public const string IpAssetManagerFriendlyId = "ipasset-manager";
        public const string RequestTimestampHeader = "Request-Timestamp";
        public const string Assets = "Assets";
        public const string Templates = "Templates";
        public const string Images = "Images";
        public const string Fonts = "Fonts";
        public const string AssistantTemplate = "AssistantTemplate.html";
        public const string SQLErrorText = "Sql Error";
        public const string IDPAuthorizationHeader = "authorization-idp";

        internal static class AssistantConfigurations
        {
            public const string ProjectContext = "PROJECT_CONTEXT";
        }

        internal static class CustomClaimTypes
        {
            public const string UserType = "user_type";
            public const string SpUrl = "sp_url";
            public const string POApiURL = "po_api_url";
        }
        internal static class AuthToken
        {
            public const string Authority = "authority";
            public const string ClientId = "client_id";
            public const string ClientSecret = "client_secret";
            public const string GrantType = "grant_type";
            public const string GrantTypePassword = "password";
            public const string GrantTypeSecret = "client_credentials";
            public const string Resource = "resource";
        }

        internal static class Chats
        {
            public static class Message
            {
                public const string NotApplicable = "The selected question is deemed not applicable to this source.";
                public const string GeneralError = "Sorry, I am unable to answer your question at the moment. Please try again later.";
                public const string RetryFailed = "Looks like something went wrong. Please ask your question again.";
                public const string RetryPending = "Looks like something went wrong. Please ask your question again.";
                public const string SourceUnavailable = "This data source is unavailable at the current time. Please try again later or contact the Capital Edge support team.";
                public const string ErrorOnWrongSQL = "I'm sorry, but I don't have enough information to answer that.  Can you please ask a more specific question or prompt?";             
            }

            public static Dictionary<string, string> Sources = new Dictionary<string, string>
            {
                { "ey-guidance", "EY Guidance" },
                { "internet", "Internet" },
                { "project-docs", "Project Documents" },
                { "project-data", "Project Data" }
            };

            public const string Summary = "Summary";
            public const string ProjectDocsSource = "project-docs";
            public const string ProjectDataSource = "project-data";
            public const string SourceKeyFormat = "SourceKey:{0}";
        }

        internal static class ContentType
        {
            public const string ApplicationJson = "application/json";
            public const string TextPlain = "text/plain";
            public const string ApplicationPdf = "application/pdf";
            public const string ApplicationPptx = "application/pptx";
            public const string ApplicationDocx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        }

        internal static class GeneratorType
        {
            public const string ChatRequest = "chat";
            public const string WorkplanRequest = "workplan";
        }
        internal static class SupportedExportFileTypes
        {
            public const string Pdf = "pdf";
            public const string Word = "docx";
        }

        internal static class ReferenceDisplayItems
        {
            public const string Documents = "Documents";
            public const string FileName = "FileName";
            public const string Table = "Table";
        }

        internal static class EYGuidanceSource
        {
            public const string HelpAssistant = "ey-guidance-help-copilot";
            public const string IP = "ey-guidance-ey-ip";
        };
        internal static class SspEndpoint
        {
            public const string IsSharepointEnabled = "projects/{0}/app-details";
            public const string TriggerWorkflow = "workflow/triggerworkflows";
            public const string AI_CONTENT_GENERATOR_WORKFLOW = "ASSISTANT_CONTENT_GENERATOR";
        }

        internal static class Redis
        {
            public const string RedisServiceName = "mymaster";
            public const string RedisSentinelPort = "26379";
            public const int RedisTimeout = 45;//Move this to configmaps

            internal static class Keys
            {
                public const string MessageKey = "user:{0}:chatid:{1}";
                //{0}=UserEmail {1}=GeneratorType
                public const string ProjectDocs = "user:{0}:projectdocs:{1}";
                public const string AppsKey = "user:{0}:apps";
            }

            internal static class OperationStatus
            {
                public const string Success = "Success";
                public const string Failure = "Failure";
            }
        }

        internal static class SharePoint
        {
            public const string ODataAnd = " and ";
            public const string DocumentsODataSelect = $"GUID,File/Name,File/Length,File/ServerRelativeUrl,File/LinkingUri,ServerRedirectedEmbedUri,{VisibleToAssistantTitle},{AssistantProcessingStatusTitle},{AssistantProcessingStatusMessageTitle},Author/EMail,Editor/EMail,Created,Modified";
            public const string DocumentsODataExpand = "File,Author,Editor";
            public const string DocumentsODataFilterAssistantProcessingStatus = $"{AssistantProcessingStatusTitle} eq '{AssistantProcessingStatusReady}'";
            public const string DocumentsODataFilterVisibleToAssistant = $"{VisibleToAssistantTitle} eq '{VisibleToAssistantYes}'";
            public const string AssistantProcessingStatusReady = "Ready";
            public const string VisibleToAssistantYes = "Yes";
            public const string VisibleToAssistantTitle = "Visible_x0020_to_x0020_Assistant";
            public const string AssistantProcessingStatusTitle = "Assistant_x0020_Processing_x0020_Status";
            public const string AssistantProcessingStatusMessageTitle = "Assistant_x0020_Processing_x0020_Status_x0020_Message";    
            public const string DocumentsODataFilterGeneratorTypeChatRequest = $"{VisibleToAssistantTitle} eq 'Chat'";
            public const string DocumentsODataFilterGeneratorTypeWorkplanRequest = $"{VisibleToAssistantTitle} eq 'Workplan Generator'";
        }
    }

    internal static class Role
    {
        public const string User = "user";
        public const string Assistant = "assistant";
    }

    internal static class ConfigMap
    {
        public const string MINIMUM_LOG_LEVEL = "MinimumLogLevel";
        public const string WORKSPACE_ID = "WorkspaceId";
        public const string WORKSPACE_NAME = "WorkspaceName";
        public const string SHARED_KEY_VAULT = "Sharedkeyvault";
        public const string APPLICATION_INSIGHTS_CONNECTION_STRING = "ApplicationInsightsConnectionString";
        public const string WORKSPACE_REGION = "WorkspaceRegion";
        public const string DATABASE_NAME = "SQLDatabaseName";
        public const string DATABASE_SERVER_NAME = "SQLServerName";
        public const string HELP_ASSISTANT_BASE_URL = "HelpAssistantBaseUrl";
        public const string IS_REDIS_UPDATE_FROM_DB_ENABLED = "RedisUpdateFromDbEnabled";
    }

    internal static class SharedKeyVault
    {
        public const string AZURE_SPN_ID = "AzureSpnId";
        public const string AZURE_SPN_SECRET = "AzureSpnSecret";
        public const string AZURE_TENANT_ID = "AzureTenantId";
        public const string SSP_BASE_URL = "SSPBaseUrl";
        public const string USER_NAME = "AadUserName";
        public const string PASSWORD = "AadPassword";
        public const string REDIS_NAME = "RedisName";
        public const string REDIS_PORT = "RedisPort";
        public const string REDIS_KEY = "RedisKey";
        public const string ORCHESTRATOR_BASE_URL = "OrchestratorBaseUrl";
        public const string ENVIRONMENT = "Environment";
        public const string REDIS_ENVIRONMENT = "RedisEnvironment";
        public const string AZURE_BLOB_STORAGE_CONNECTION_STRING = "SSPBlobStorage:ConnectionString";
        public const string GEMBOX_DOCUMENT_LICENSE_KEY = "GemBoxDocumentLicenseKey";
        public const string GEMBOX_PRESENTATION_LICENSE_KEY = "GemBoxPresentationLicenseKey";
        public const string OPEN_AI_API_KEY = "ce-assistant-open-ai-api-key";
        public const string OPEN_AI_API_ENDPOINT = "ce-assistant-open-ai-endpoint";
        public const string OPEN_AI_EMBEDDING_DEPLOYMENT = "ce-assistant-open-ai-embedding-deployment";
    }

    internal static class RunTimeStatus
    {
        public const string Completed = "completed";
        public const string Failed = "failed";
        public const string Pending = "pending";
        public const string InProgress = "in progress";
        public const string Aborted = "aborted";
    }

    public static class PortalEndPoints
    {
        public const string GetProjectDetails = "/api/v1/sharedservices/projects/{0}";
        public const string PatchProjectDetails = "/api/v1/projects/{0}/update";
        public const string GetUserDetails = "/api/v1/sharedservices/users/{0}?includeUserPhoto=true";
        public const string GetAppsList = "/api/v1/projects/{0}";
    }

    public static class HelpAssistantEndpoints
    {
        public const string GetDocument = "/api/Documents/{0}/download";
    }

    public static class ProgramOfficeEndPoints
    {
        public const string GetFileInfoByDocumentId = "/{0}-ce4/api/copilot/file-info/{1}";

        public const string ExecuteQuery = "/copilot/execute-query";
    }

    public static class OrchestratorEndpoints
    {
        public const string PostChat = "ceassistant-orchestrator-initialize/chat";
    }

    public static class ContentGeneratorEndpoint
    {
        public const string BaseUrl = "http://content-generator.{0}:80";
        public const string TerminateProjectStatus= "pmo-status-report-generator/terminate/{0}";
        public const string TerminateWorkplan = "pmo-workplan-generator/terminate/{0}";
        public const string TerminateProjectCharter = "project-charter-generator/terminate/{0}";
    }
}
