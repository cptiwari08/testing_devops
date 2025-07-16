using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantAPITest.Dependency
{
    internal class ConfigurationDependency
    {
        Dictionary<string,string> inMemorySettings = new Dictionary<string, string> {
            {"OrchestratorBaseUrl", "https://eycapitaledgeapim-dev.ey.com/"},
            {"SSPBaseUrl", "https://eycapitaledge-dev.ey.com"},
        };  

        internal IConfiguration configuration;
        internal ConfigurationDependency()
        {
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
    }
}
