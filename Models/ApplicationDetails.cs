using Newtonsoft.Json;

namespace WebApplicationForm.Models
{
    public class ApplicationDetails
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("TenantId")]
        public string TenantId { get; set; }

        [JsonProperty("personalInformation")]
        public PersonalInformation PersonalInformation { get; set; }

        [JsonProperty("answers")]
        public List<Answer> Answers { get; set; }

        [JsonProperty("hasBeenRejectedByUKEmbassy")]
        public bool HasBeenRejectedByUKEmbassy { get; set; }

        [JsonProperty("rejectionExplanation")]
        public string RejectionExplanation { get; set; }
    }
}
