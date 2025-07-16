using BusinessLogic;
using Contract.Interfaces.Product;
using DataAccess;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Text.Json;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureOpenApi()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.Configure<JsonSerializerOptions>(options => {
            options.PropertyNameCaseInsensitive = true;
        });

        services.AddScoped<IProductBL, ProductBL>();
        //services.AddSingleton<IProductDA, ProductDA>();
        services.AddSingleton<IProductDA, ProductDADapper>();

        services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
        {
            var options = new OpenApiConfigurationOptions()
            {
                Info = new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "ServerlessAPI",
                    Description = "This document contains the API details for ServerlessAPI",
                    TermsOfService = new Uri("http://opensource.org/licenses/MIT"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Daniel",
                        Email = "Daniel.Rivera@ey.com",
                        Url = new Uri("http://opensource.org/licenses/MIT"),
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("http://opensource.org/licenses/MIT"),
                    },
                },
                Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                IncludeRequestingHostName = true,
                ForceHttps = false,
                ForceHttp = false,
                OpenApiVersion = OpenApiVersionType.V3,

            };
            return options;
        });
    })
    .Build();

host.Run();
