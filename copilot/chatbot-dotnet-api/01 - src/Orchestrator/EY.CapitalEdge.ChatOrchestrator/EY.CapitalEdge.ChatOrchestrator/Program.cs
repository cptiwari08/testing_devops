using EY.CapitalEdge.ChatOrchestrator.Services;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Azure.Identity;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace EY.CapitalEdge.ChatOrchestrator
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = new HostBuilder()
            .ConfigureFunctionsWebApplication()
            .ConfigureServices((context, services) =>
            {
                IConfiguration config = context.Configuration;
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();
                services.AddHttpClient();

                services.Configure<LoggerFilterOptions>(options =>
                {
                    // The Application Insights SDK adds a default logging filter that instructs ILogger to capture only Warning and more severe logs. Application Insights requires an explicit override.
                    // Log levels can also be configured using appsettings.json. For more information, see https://learn.microsoft.com/en-us/azure/azure-monitor/app/worker-service#ilogger-logs
                    LoggerFilterRule? toRemove = options.Rules.FirstOrDefault(rule => rule.ProviderName
                        == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");

                    if (toRemove is not null)
                    {
                        options.Rules.Remove(toRemove);
                    }
                });

                string? sharedKeyVault = config["KEY_VAULT_URL"];
                if (string.IsNullOrWhiteSpace(sharedKeyVault))
                    throw new ArgumentException($"KEY_VAULT_URL is not set");

                string? tenantId = config["AZURE_TENANT_ID"];
                if (string.IsNullOrWhiteSpace(tenantId))
                    throw new ArgumentException($"AZURE_TENANT_ID is not set");

                string? clientId = config["AZURE_CLIENT_ID"];
                if (string.IsNullOrWhiteSpace(clientId))
                    throw new ArgumentException($"AZURE_CLIENT_ID is not set");

                string? clientSecret = config["AZURE_CLIENT_SECRET"];
                if (string.IsNullOrWhiteSpace(clientSecret))
                    throw new ArgumentException($"AZURE_CLIENT_SECRET is not set");

                var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                services.AddAzureClients(builder =>
                {
                    builder.UseCredential(clientSecretCredential);
                    builder.AddSecretClient(new Uri(sharedKeyVault));
                });

                services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
                {
                    var options = new OpenApiConfigurationOptions()
                    {
                        Info = new OpenApiInfo()
                        {
                            Version = "v1",
                            Title = "ChatBotOrchestrator",
                            Description = "This service answer questions based on sources and context provided"
                        },
                        Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                        IncludeRequestingHostName = true,
                        ForceHttps = false,
                        ForceHttp = false,
                        OpenApiVersion = OpenApiVersionType.V3,
                    };
                    return options;
                });

                services.AddSingleton<IOpenApiHttpTriggerAuthorization>(_ =>
                {
                    var auth = new OpenApiHttpTriggerAuthorization(req =>
                    {
                        var result = default(OpenApiAuthorizationResult);
                        return Task.FromResult(result);
                    });

                    return auth;
                });

                services.AddSingleton<IOpenAIServiceFactory, OpenAIServiceFactory>();
                services.AddSingleton<ISearchClientFactory, SearchClientFactory>();

                services.AddScoped<Services.ILoggerProvider, LoggerProvider>();
                services.AddSingleton<ICommon, Common>();
                services.AddTransient<IDurableTaskClientWrapper, DurableTaskClientWrapper>();
                services.AddSingleton<ISuggestion, Suggestion>();
                services.AddScoped<IEmbedding, Embedding>();
                services.AddSingleton<IMmrQuestionSelectorWithEmbedding, MmrQuestionSelectorWithEmbedding>();
                services.AddTransient<IStopwatch, StopwatchWrapper>();         
            })
            .Build();

            host.Run();
        }
    }
}