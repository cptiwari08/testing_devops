using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Mapper;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using Microsoft.EntityFrameworkCore;

namespace EY.CE.Copilot.API.Clients
{
    /// <summary>
    /// Client class for accessing and updating Copilot configurations.
    /// </summary>
    public class ConfigurationClient : IConfigurationClient
    {
        private readonly Data.Contexts.CopilotContext _context;
        private readonly ILoggerHelperSingleton _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationClient"/> class.
        /// </summary>
        /// <param name="context">The Copilot context.</param>
        /// <param name="logger">The logger helper singleton.</param>
        public ConfigurationClient(Data.Contexts.CopilotContext context, ILoggerHelperSingleton logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Gets the Copilot configuration for the specified key.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>The Copilot configuration.</returns>
        public async Task<CopilotConfiguration> GetConfiguration(string key)
        {
            try
            { 
                return await _context.CopilotConfigurations.AsNoTracking().FirstOrDefaultAsync(col => col.Key.ToLower() == key.ToLower());
            }
            catch (Exception e)
            {
                _logger.Log(AppLogLevel.Error, $"Unable to get configuration {e.Message}", nameof(ConfigurationClient), nameof(GetConfiguration), exception: e);
                throw;
            }
        }

        /// <summary>
        /// Updates the specified Copilot configuration.
        /// </summary>
        /// <param name="configuration">The Copilot configuration to update.</param>
        public async Task UpdateConfiguration(CopilotConfiguration configuration)
        {
            try
            {
                _context.CopilotConfigurations.Update(configuration);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Log(AppLogLevel.Error, $"Unable to update configuration {e.Message}", nameof(ConfigurationClient), nameof(UpdateConfiguration), exception: e);
                throw;
            }
        }

        public async Task UpdateConfiguration(ConfigurationUpdate configuration)
        {
            try
            {
                var dbConfiguration = _context.CopilotConfigurations.Where(s => s.ID == configuration.ID).FirstOrDefault();
                if (dbConfiguration != null)
                {
                    ConfigurationMapper.CreateUpdateModelForConfiguration(configuration, dbConfiguration);
                    _context.CopilotConfigurations.Update(dbConfiguration);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.Log(AppLogLevel.Error, $"Unable to perform update in configuration {e.Message}", nameof(ConfigurationClient), nameof(UpdateConfiguration), exception: e);
                throw;
            }
        }
    }
}