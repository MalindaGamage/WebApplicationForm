namespace WebApplicationForm.DTO
{
    public class ResponseDto
    {
        public string QuestionId { get; set; }
        public string AnswerText { get; set; }
        public List<string> SelectedOptions { get; set; }
    }
}
