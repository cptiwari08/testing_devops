namespace EY.CapitalEdge.HelpCopilot.Static
{
    public static class Constants
    {
        public const string Authorization = "Authorization";
        public const string CEAuthHeader = "ce-auth";
        public const string CEAuthHeaderValue = "";
    }

    internal static class ConfigMap
    {
        public const string KeyVaultUrl = "KEY_VAULT_URL";
    }

    internal static class SharedKeyVault
    {
        public const string AzureClientId = "AZURE_CLIENT_ID";
        public const string AzureClientSecret = "AZURE_CLIENT_SECRET";
        public const string AzureTenantId = "AZURE_TENANT_ID";
    }

    internal static class SaTKnowledgeAssistantWrapper
    {
        public const string TimeoutPolicy = "satKnowledgeAssistantWrapperTimeoutPolicy";
        public const string RetryCountPolicy = "satKnowledgeAssistantWrapperRetryCountPolicy";
    }

    public static class SaTKnowledgeAssistant
    {
        public const string BaseAddress = "satKnowledgeAssistantBaseAddress";
        public const string Session = "/api/User/session";
        public const string StartConversation = "/api/Chat/start-conversation";
        public const string Chat = "/api/Chat";
        public const string ChatSensitiveDataSupport = "/api/Chat/SensitiveDataSupport";
    }

    internal static class ContentType
    {
        public const string ApplicationJson = "application/json";
    }
}
