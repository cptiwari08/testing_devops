using EY.CE.Copilot.API.Static;
using System.ComponentModel.DataAnnotations;

namespace EY.CE.Copilot.API.Models
{
    public class ConfigurationUpdate
    {
        public int ID { get; set; }
        [SpecialCharNotAllowed()]
        [ScriptNotAllowed()]
        [StringLength(50)]
        public string? Title { get; set; }

        public string? Key { get; set; }

        [StringLength(5000)]
        public string? Value { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
