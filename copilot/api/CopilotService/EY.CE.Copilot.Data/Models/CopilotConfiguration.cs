using System.ComponentModel.DataAnnotations.Schema;
namespace EY.CE.Copilot.Data.Models
{
    [Table("AssistantConfigurations")]
    public class CopilotConfiguration
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsEnabled { get; set; }
    }
}
