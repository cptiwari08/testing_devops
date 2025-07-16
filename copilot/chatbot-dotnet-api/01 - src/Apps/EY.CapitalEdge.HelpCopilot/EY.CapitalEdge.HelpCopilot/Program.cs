using EY.CapitalEdge.HelpCopilot.Services;
using EY.CapitalEdge.HelpCopilot.Utils;
using Polly;
using Polly.Extensions.Http;
using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using EY.CapitalEdge.HelpCopilot.Static;
using System.Reflection;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace EY.CapitalEdge.HelpCopilot
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
            }

            string? sharedKeyVault = builder.Configuration[ConfigMap.KeyVaultUrl];
            if (string.IsNullOrWhiteSpace(sharedKeyVault))
                throw new ArgumentException($"{ConfigMap.KeyVaultUrl} is not set");

            if (builder.Environment.IsDevelopment())
            {
                Console.WriteLine($"connecting to azure key vault {sharedKeyVault} using spn in local");
                builder.Configuration.AddAzureKeyVault(new Uri(sharedKeyVault),
                    new ClientSecretCredential(
                        builder.Configuration[SharedKeyVault.AzureTenantId],
                        builder.Configuration[SharedKeyVault.AzureClientId],
                        builder.Configuration[SharedKeyVault.AzureClientSecret]));
            }
            else
            {
                Console.WriteLine($"connecting to azure key vault {sharedKeyVault} using DefaultAzureCredential");
                builder.Configuration.AddAzureKeyVault(new Uri(sharedKeyVault),
                    new DefaultAzureCredential());
            }

            // Environment variable: APPLICATIONINSIGHTS_CONNECTION_STRING
            var options = new ApplicationInsightsServiceOptions { ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"] };
            builder.Services.AddApplicationInsightsTelemetry(options: options);

            builder.Services.Configure<LoggerFilterOptions>(options =>
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

            int timeoutSeconds = int.TryParse(builder.Configuration[SaTKnowledgeAssistantWrapper.TimeoutPolicy], out int parsedTimeout) ? parsedTimeout : 60;
            int retryCount = int.TryParse(builder.Configuration[SaTKnowledgeAssistantWrapper.RetryCountPolicy], out int parsedRetryCount) ? parsedRetryCount : 3;
            
            // Add services to the container.
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(timeoutSeconds);
            var retryPolicy = HttpPolicyExtensions
              .HandleTransientHttpError() // HttpRequestException, 5XX and 408
              .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            builder.Services.AddHttpClient<IHelpCopilotService, HelpCopilotService>()
              .AddPolicyHandler(timeoutPolicy)
              .AddPolicyHandler(retryPolicy);

            builder.Services.AddSingleton<ICommon, Common>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}