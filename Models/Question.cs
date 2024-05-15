using Newtonsoft.Json;

namespace WebApplicationForm.Models
{
    public class Question
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } // "Paragraph", "YesNo", "Dropdown", "MultipleChoice", "Date", "Number"

        [JsonProperty("options")]
        public List<string> Options { get; set; } = new List<string>();

        [JsonProperty("minValue")]
        public int MinValue { get; set; }

        [JsonProperty("maxValue")]
        public int MaxValue { get; set; }

        [JsonProperty("placeholder")]
        public string Placeholder { get; set; }
    }
}
