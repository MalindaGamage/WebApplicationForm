using Newtonsoft.Json;

namespace WebApplicationForm.Models
{
    public class PersonalInformation
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("currentResidence")]
        public string CurrentResidence { get; set; }

        [JsonProperty("idNumber")]
        public string IDNumber { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateOnly DateOfBirth { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }
    }
}
