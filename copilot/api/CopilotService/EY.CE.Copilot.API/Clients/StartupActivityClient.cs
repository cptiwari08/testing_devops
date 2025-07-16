using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static EY.CE.Copilot.API.Static.Constants;
using static EY.CE.Copilot.API.Static.Constants.Chats;

namespace EY.CE.Copilot.API.Clients
{
    public class StartupActivityClient : BaseClass, IStartupActivityClient
    {
        private readonly Data.Contexts.CopilotContext _context;
        private readonly IAppLoggerService _logger;
        private readonly IConfigurationClient _configurationClient;
        private readonly IPortalClient _portalClient;
        
        public StartupActivityClient(Data.Contexts.CopilotContext context,
        IConfigurationClient configurationClient, IPortalClient portalClient, IAppLoggerService logger) : base(logger, nameof(PortalClient))
        {
            _context = context;
            _configurationClient = configurationClient;
            _logger = logger;
            _portalClient = portalClient;
        }

        public async Task<bool> Execute()
        {
            try
            {
                return await UpdateProjectContext();                
            }
            catch (Exception e)
            {
                _logger.Log(AppLogLevel.Error, $"Unable to execute activities {e.Message}", nameof(StartupActivityClient), nameof(Execute), exception: e);
                return false;
            }
        }

        private async Task<bool> UpdateProjectContext()
        {
            try
            {
                var config = await _configurationClient.GetConfiguration(AssistantConfigurations.ProjectContext);
                if (config != null)
                {
                    var portalconfig = await _portalClient.GetProjectDetails();
                    if (portalconfig != null)
                    {
                        var projectContext = GenerateProjectContextString(portalconfig.Data.IsConfidential, portalconfig.Data.Name, portalconfig.Data.ClientSize, portalconfig.Data.Area, portalconfig.Data.Region, portalconfig.Data.Country, portalconfig.Data.Type, portalconfig.Data.Sector);
                        if (string.IsNullOrEmpty(config.Value))
                        {
                            config.Value = projectContext;
                            await _configurationClient.UpdateConfiguration(config);
                        }
                    }

                }
               
            }
            catch (Exception e)
            {
                _logger.Log(AppLogLevel.Error, $"Unable to update project context {e.Message}", nameof(StartupActivityClient), nameof(UpdateProjectContext), exception: e);
                return false;
            }
            return true;
        }

        public string GenerateProjectContextString(bool isConfidential, string clientName, string clientSize, string area, string region, string country, string type, string sector)
        {
            return $"{(!isConfidential && string.IsNullOrEmpty(clientName)  ? "" : $"Name of the client is {clientName}.\n")}" +
                   $"{(string.IsNullOrEmpty(clientSize) ? "" : $"Annual revenue of the client is {clientSize}.\n")}" +
                   $"{(string.IsNullOrEmpty(area) ? "" : $"Area the client is located in is {area}.\n")}" +
                   $"{(string.IsNullOrEmpty(region) ? "" : $"Region the client is located in is {region}.\n")}" +
                   $"{(string.IsNullOrEmpty(country) ? "" : $"Country the client is located in is {country}.\n")}" +
                   $"{(string.IsNullOrEmpty(type) ? "" : $"This is a {type} type of project.\n")}" +
                   $"{(string.IsNullOrEmpty(sector) ? "" : $"The client is in the {sector} sector.\n")}";
        }



    }
}
