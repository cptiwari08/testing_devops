using EY.CE.Copilot.API.Models;

namespace EY.CE.Copilot.API.Mapper
{
    public class ConfigurationMapper
    {
        public static void CreateUpdateModelForConfiguration(ConfigurationUpdate apiConfiguration, Data.Models.CopilotConfiguration dbConfiguration)
        {
            dbConfiguration.Title = apiConfiguration.Title ?? dbConfiguration.Title;
            dbConfiguration.Key = apiConfiguration.Key ?? dbConfiguration.Key;
            dbConfiguration.Value = apiConfiguration.Value ?? dbConfiguration.Value;
            dbConfiguration.IsEnabled = apiConfiguration.IsEnabled ?? dbConfiguration.IsEnabled;
        }
    }
}
