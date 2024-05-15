namespace WebApplicationForm.DTO
{
    public class QuestionDto
    {
        public string? Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; } // "Paragraph", "YesNo", "Dropdown", "MultipleChoice", "Date", "Number"
        public List<string> Options { get; set; } // For Dropdown and MultipleChoice
        public int MinValue { get; set; } // For Number
        public int MaxValue { get; set; } // For Number
        public string Placeholder { get; set; } // For Paragraph
    }
}
