using OpenAI;
using OpenAI.Interfaces;
using OpenAI.Managers;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    [ExcludeFromCodeCoverage]
    public class OpenAIServiceFactory : IOpenAIServiceFactory
    {
        private readonly Dictionary<string, IOpenAIService> _services;

        public OpenAIServiceFactory()
        {
            // Initialize and configure your OpenAIService instances based on the configuration
            _services = new Dictionary<string, IOpenAIService>
            {
                { "GenericOpenAIService", new OpenAIService(new OpenAiOptions
                    {
                        ApiKey = Environment.GetEnvironmentVariable("GenericOpenAIServiceOptionsApiKey")
                            ?? throw new ArgumentException("GenericOpenAIServiceOptionsApiKey is not set"),
                        DeploymentId = Environment.GetEnvironmentVariable("GenericOpenAIServiceOptionsDeploymentId")
                            ?? throw new ArgumentException("GenericOpenAIServiceOptionsDeploymentId is not set"),
                        BaseDomain = Environment.GetEnvironmentVariable("GenericOpenAIServiceOptionsBaseDomain")
                            ?? throw new ArgumentException("GenericOpenAIServiceOptionsBaseDomain is not set"),
                        ProviderType = ProviderType.Azure,
                        ApiVersion = Environment.GetEnvironmentVariable("GenericOpenAIServiceOptionsApiVersion")
                            ?? throw new ArgumentException("GenericOpenAIServiceOptionsApiVersion is not set")
                    })
                },
                { "EmbeddingOpenAIService", new OpenAIService(new OpenAiOptions
                    {
                        ApiKey = Environment.GetEnvironmentVariable("EmbeddingOpenAIServiceOptionsApiKey")
                            ?? throw new ArgumentException("EmbeddingOpenAIServiceOptionsApiKey is not set"),
                        DeploymentId = Environment.GetEnvironmentVariable("EmbeddingOpenAIServiceOptionsDeploymentId")
                            ?? throw new ArgumentException("EmbeddingOpenAIServiceOptionsDeploymentId is not set"),
                        BaseDomain = Environment.GetEnvironmentVariable("EmbeddingOpenAIServiceOptionsBaseDomain")
                            ?? throw new ArgumentException("EmbeddingOpenAIServiceOptionsBaseDomain is not set"),
                        ProviderType = ProviderType.Azure,
                        ApiVersion = Environment.GetEnvironmentVariable("EmbeddingOpenAIServiceOptionsApiVersion")
                            ?? throw new ArgumentException("EmbeddingOpenAIServiceOptionsApiVersion is not set")
                    }) 
                }
        };
        }

        public IOpenAIService GetService(string key)
        {
            if (_services.TryGetValue(key, out var service))
            {
                return service;
            }

            throw new KeyNotFoundException($"Service with key {key} not found.");
        }
    }
}
