using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using EY.CE.Copilot.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EY.CE.Copilot.API.Controllers
{
    [Authorize(Policy = AuthenticationPolicy.CE_USER_POLICY)]
    [Route("[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationClient _client;
        private readonly IPortalClient _portalClient;

        public ConfigurationController(IConfigurationClient client, IPortalClient portalClient)
        {
            _client = client;
            _portalClient = portalClient;
        }

        /// <summary>
        /// Retrieves the configuration value for the specified key.
        /// </summary>
        /// <param name="key">The key of the configuration value to retrieve.</param>
        /// <returns>The configuration value.</returns>
        [Authorize(Policy = AuthenticationPolicy.CE_USER_OR_APISECRET)]
        [HttpGet]
        [Route("{key}")]
        public async Task<IActionResult> GetConfiguration(string key)
        {
            var (client, configKey) = GetConfigurationClient(key);
            var response = await client.GetConfiguration(configKey);
            return Ok(response);
        }

        /// <summary>
        /// Updates the configuration value.
        /// </summary>
        /// <param name="configuration">The updated configuration value.</param>
        /// <returns>An HTTP status code indicating the result of the update operation.</returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] ConfigurationUpdate configuration)
        {
            try
            {
                if (configuration == null)
                {
                    return BadRequest();
                }
                var (client, configKey) = GetConfigurationClient(configuration.Key);
                configuration.Key = configKey;

                if (configuration.Key.ToLower().Equals("source_configs"))
                {
                    var sourceConfigs = JsonConvert.DeserializeObject<List<SourceConfiguration>>(configuration.Value);
                    foreach (var item in sourceConfigs)
                    {
                        if (item.DisplayName.Length > 15 || item.OriginalDisplayName.Length > 15 || item.Key.Length > 15)
                            return BadRequest("Souce Name size cannot be more then 15");
                    }
                    var copilotConfiguration = await client.GetConfiguration("source_configs");
                    var existingSourceConfigs = JsonConvert.DeserializeObject<List<SourceConfiguration>>(copilotConfiguration.Value);
                    foreach (var existingConfig in existingSourceConfigs)
                    {
                        var newConfig = sourceConfigs?.FirstOrDefault(c => c.Key == existingConfig.Key);
                        if (newConfig != null)
                        {
                            existingConfig.DisplayName = newConfig.DisplayName;
                            existingConfig.Ordinal = newConfig.Ordinal;
                            existingConfig.IsDefault = newConfig.IsDefault;
                            existingConfig.Description = newConfig.Description;
                            existingConfig.IsActive = newConfig.IsActive;
                            existingConfig.IsQuestionTextBoxEnabled = newConfig.IsQuestionTextBoxEnabled;
                        }
                    }
                    configuration.Value = JsonConvert.SerializeObject(existingSourceConfigs);
                }
                await client.UpdateConfiguration(configuration);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Determines the appropriate configuration client and configuration key based on the provided key.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>A tuple containing the selected configuration client and the processed configuration key.</returns>
        private (IConfig client, string configKey) GetConfigurationClient(string key)
        {
            string configKey = key.Replace(Constants.PortalConfigurationPrefix, string.Empty);
            IConfig client = key.StartsWith(Constants.PortalConfigurationPrefix) ? _portalClient : _client;

            return (client, configKey);
        }
    }
}