using Azure.Identity;
using EY.CE.Copilot.API.Authorization;
using EY.CE.Copilot.API.Clients;
using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Extensions;
using EY.CE.Copilot.API.Handler;
using EY.CE.Copilot.API.Middleware;
using EY.CE.Copilot.API.Static;
using EY.CE.Copilot.Data.Contexts;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using EY.SaT.CapitalEdge.Extensions.Logging.Services;
using EY.SAT.CE.CoreServices.DependencyInjection;
using EY.SAT.CE.CoreServices.Implementations;
using EY.SAT.CE.SharePoint.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using static EY.CE.Copilot.API.Static.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Configure the app to use environment-specific appsettings files
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
string? sharedKeyVault = builder.Configuration[ConfigMap.SHARED_KEY_VAULT];
if (!builder.Environment.IsDevelopment())
{
    Console.WriteLine($"connecting to azure key vault {sharedKeyVault} using managed identity");
    builder.Configuration.AddAzureKeyVault(new Uri($"https://{sharedKeyVault}.vault.azure.net/"),
        new DefaultAzureCredential());
}
else
{
    //connect azure key vault using spn in local development.
    Console.WriteLine($"connecting to azure key vault {sharedKeyVault} using spn in local");
    builder.Configuration.AddAzureKeyVault(new Uri($"https://{sharedKeyVault}.vault.azure.net/"),
               new ClientSecretCredential(builder.Configuration[SharedKeyVault.AZURE_TENANT_ID],
                          builder.Configuration[SharedKeyVault.AZURE_SPN_ID],
                                     builder.Configuration[SharedKeyVault.AZURE_SPN_SECRET]));
}
// Add services to the container.
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IRedisClient>(serviceProvider =>
        new RedisClient(builder.Configuration));
AddHttpClients(builder);
builder.Services.AddSingleton<IAppLogger>(ServiceProvider =>
        new AppLogger(
          builder.Configuration[ConfigMap.APPLICATION_INSIGHTS_CONNECTION_STRING],
          builder.Configuration[ConfigMap.MINIMUM_LOG_LEVEL]));
builder.Services.AddSingleton<ILoggerHelperSingleton>(serviceProvider =>
        new LoggerHelperSingleton(builder.Configuration, serviceProvider.GetRequiredService<IAppLogger>()));
builder.Services.AddScoped<IStartupActivityClient, StartupActivityClient>();
builder.Services.AddScoped<IAppLoggerService>(serviceProvider =>
        new AppLoggerService(serviceProvider.GetRequiredService<IAppLogger>(),
        LoggerHelper.CreateLoggerContext(serviceProvider.GetRequiredService<IHttpContextAccessor>(), builder.Configuration)));
builder.Services.AddSingleton<IAuthentication, Authentication>();
builder.Services.AddScoped<IPortalClient, PortalClient>();
builder.Services.AddScoped<IProgramOfficeClient, ProgramOfficeClient>();
builder.Services.AddScoped<ISessionClient, SessionClient>();
builder.Services.AddScoped<ISuggestionsClient, SuggestionsClient>();
builder.Services.AddScoped<IOrchestratorClient, OrchestratorClient>();
builder.Services.AddScoped<IChatClient, ChatClient>();
builder.Services.AddScoped<IDocumentsClient, DocumentsClient>();
builder.Services.AddScoped<IExportFileClient, ExportFileClient>();
builder.Services.AddScoped<IGlossaryClient, GlossaryClient>();
builder.Services.AddScoped<IPromptClient, PromptClient>();
builder.Services.AddDbContext<CopilotContext>(options => options.UseSqlServer($@"Server={builder.Configuration[ConfigMap.DATABASE_SERVER_NAME]}.database.windows.net; Authentication=Active Directory Password; Database={builder.Configuration[ConfigMap.DATABASE_NAME]}; User Id={builder.Configuration[SharedKeyVault.USER_NAME]}; Password={builder.Configuration[SharedKeyVault.PASSWORD]}"));
builder.Services.AddScoped<IConfigurationClient, ConfigurationClient>();
builder.Services.AddScoped<IContentGeneratorClient,ContentGeneratorClient>();
builder.Services.AddScoped<ISearchClient, SearchClient>();
builder.Logging.AddConsole();
builder.Services.AddAuthorizationCore(
                options =>
                {
                    options.AddPolicy(AuthenticationPolicy.CE_USER_POLICY, policy =>
                    {
                        policy.AddAuthenticationSchemes(CEAuthExtension.JWTAuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                    });
                    options.AddPolicy(AuthenticationPolicy.CE_USER_OR_APISECRET, policy =>
                    {
                        policy.AddAuthenticationSchemes(CEAuthExtension.SecretAuthenticationScheme, 
                            CEAuthExtension.JWTAuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                    });
                }
            );
builder.Services.AddCEAuthentication(builder.Configuration, cEAuthenticationOptions: new CEAuthenticationOptions
{
    TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration[ConfigMap.WORKSPACE_ID]
    }
}, defaultAuthenticationScheme: CEAuthExtension.JWTAuthenticationScheme, apiAuthKeyConfigName: "Ce-API-Key-Po");

builder.Services.AddHttpContextAccessor();
if (builder.Configuration[SharedKeyVault.ENVIRONMENT] == DevelopmentEnvironment)
{
    Console.WriteLine("adding dev env cors");
    builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
}
builder.Services.AddCETokenGeneration();
builder.Services.AddCeSharePoint(builder.Configuration[SharedKeyVault.SSP_BASE_URL] + "/api/v1/", options =>
{
    options.SspProjectId = builder.Configuration[ConfigMap.WORKSPACE_ID];
});
builder.Services.AddSingleton<IFireForgetRepositoryHandler, FireForgetRepositoryHandler>();
builder.Services.AddControllers()
    .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
var app = builder.Build();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<UserAuthorization>();
app.UseMiddleware<AntiXSS>();
app.UseAuthorization();
//Comment this code when running this in Visual studio in local as without docker bitnami redis, image wont be created for use.
IRedisClient redisClient = app.Services.GetRequiredService<IRedisClient>();
redisClient.ConnectRedisCache();
using (var scope = app.Services.CreateScope())
{
    var startupActivityClient = scope.ServiceProvider.GetRequiredService<IStartupActivityClient>();
    await startupActivityClient.Execute();
}

app.MapControllers();
app.Run();

static void AddHttpClients(WebApplicationBuilder builder)
{
    builder.Services.AddHttpClient(SSPHttpClient, client =>
    {
        client.BaseAddress = new Uri(builder.Configuration[SharedKeyVault.SSP_BASE_URL]+"/");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.ApplicationJson));
    }).AddWaitAndRetryAsync();
    builder.Services.AddHttpClient(ProgramOfficeHttpClient, client =>
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.ApplicationJson));
        client.DefaultRequestHeaders.Add(CEAuthHeader, CEAuthHeaderValue);
        client.DefaultRequestHeaders.Add(RequestTimestampHeader, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
    }).AddWaitAndRetryAsync();
    builder.Services.AddHttpClient(ContentGeneratorHttpClient, client =>
    {
        client.BaseAddress = new Uri(string.Format(ContentGeneratorEndpoint.BaseUrl, builder.Configuration[ConfigMap.WORKSPACE_NAME]) + "/");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.ApplicationJson));
    }).AddWaitAndRetryAsync();
}