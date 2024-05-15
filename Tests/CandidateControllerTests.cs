using ApplicationForm.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplicationForm;
using WebApplicationForm.DTO;
using WebApplicationForm.Models;
using Xunit;

public class CandidateControllerTests
{
    private readonly Mock<ICosmosDbService> _mockCosmosDbService;
    private readonly Mock<IApplicationDetailsService> _mockApplicationDetailsService;
    private readonly CandidateController _controller;

    public CandidateControllerTests()
    {
        _mockCosmosDbService = new Mock<ICosmosDbService>();
        _mockApplicationDetailsService = new Mock<IApplicationDetailsService>();
        _controller = new CandidateController(_mockCosmosDbService.Object, _mockApplicationDetailsService.Object);
    }

    [Fact]
    public async Task GetQuestions_ReturnsOkResult_WithListOfQuestions()
    {
        // Arrange
        var questions = new List<Question>
        {
            new Question { Content = "Question 1", Type = "SingleChoice", Options = new List<string> { "Option1", "Option2" }, MinValue = 0, MaxValue = 0, Placeholder = "Choose" },
            new Question { Content = "Question 2", Type = "MultipleChoice", Options = new List<string> { "Option1", "Option2" }, MinValue = 0, MaxValue = 0, Placeholder = "Choose" }
        };
        _mockCosmosDbService.Setup(s => s.GetQuestionsAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                            .ReturnsAsync(questions);

        // Act
        var result = await _controller.GetQuestions(null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnQuestions = Assert.IsType<List<QuestionDto>>(okResult.Value);
        Assert.Equal(2, returnQuestions.Count);
    }

    [Fact]
    public async Task SubmitResponses_ReturnsOkResult()
    {
        // Arrange
        var responses = new List<ResponseDto>
        {
            new ResponseDto { QuestionId = "1", AnswerText = "Answer 1", SelectedOptions = new List<string> { "Option1" } }
        };

        // Act
        var result = await _controller.SubmitResponses(responses);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SubmitApplication_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var applicationDetails = new ApplicationDetails { TenantId = "10", PersonalInformation = new PersonalInformation() };
        var createdDetails = new ApplicationDetails { Id = "1", TenantId = "10" };
        _mockApplicationDetailsService.Setup(s => s.CreateAsync(It.IsAny<ApplicationDetails>()))
                                      .ReturnsAsync(createdDetails);

        // Act
        var result = await _controller.SubmitApplication(applicationDetails);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnDetails = Assert.IsType<ApplicationDetails>(createdResult.Value);
        Assert.Equal("1", returnDetails.Id);
    }

    [Fact]
    public async Task GetApplicationById_ReturnsNotFound_WhenDetailsDoNotExist()
    {
        // Arrange
        _mockApplicationDetailsService.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                                      .ReturnsAsync((ApplicationDetails)null);

        // Act
        var result = await _controller.GetApplicationById("1", "10");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetApplicationById_ReturnsOkResult_WithDetails()
    {
        // Arrange
        var applicationDetails = new ApplicationDetails { Id = "1", TenantId = "10", PersonalInformation = new PersonalInformation() };
        _mockApplicationDetailsService.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                                      .ReturnsAsync(applicationDetails);

        // Act
        var result = await _controller.GetApplicationById("1", "10");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnDetails = Assert.IsType<ApplicationDetails>(okResult.Value);
        Assert.Equal("1", returnDetails.Id);
    }
}
