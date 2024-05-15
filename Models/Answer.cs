using Newtonsoft.Json;

namespace WebApplicationForm.Models
{
    public class Answer
    {
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

        [JsonProperty("selectedOptions")]
        public List<string> SelectedOptions { get; set; }  // Applicable for dropdowns and multiple choice

        [JsonProperty("textAnswer")]
        public string AnswerText { get; set; }  // Applicable for text input or text area
    }
}
