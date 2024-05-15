namespace WebApplicationForm.DTO
{
    public class ApplicationDetailsDto
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public PersonalInformationDto PersonalInformation { get; set; }
        public List<ResponseDto> Answers { get; set; }
        public bool HasBeenRejectedByUKEmbassy { get; set; }
        public string RejectionExplanation { get; set; }
    }
}
