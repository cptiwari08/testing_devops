using Newtonsoft.Json;

namespace EY.CE.Copilot.Data.Models
{
    internal class SuggestionMatrix
    {
        [JsonProperty("Source")]
        public string Source { get; set; }

        [JsonProperty("Suggestions")]
        public List<SuggestionData> Suggestions { get; set; }

        [JsonProperty("AppAfinity")]
        public string AppAfinity { get; set; }
    }

    internal class SuggestionData
    {
        public string SuggestionText { get; set; }
        public string SQLQuery { get; set; }
        private bool _VisibleToAssistant = true;
        private bool _IsIncluded = true;
        public bool VisibleToAssistant {
            get
            {
                return _VisibleToAssistant;
            }
            set
            {
                _VisibleToAssistant = value;
            }
        }
        public bool IsIncluded
        {
            get
            {
                return _IsIncluded;
            }
            set
            {
                _IsIncluded = value;
            }
        }
    }
}
