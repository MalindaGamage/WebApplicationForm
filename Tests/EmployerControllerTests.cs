using ApplicationForm.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplicationForm;
using WebApplicationForm.DTO;
using WebApplicationForm.Models;
using Xunit;

public class EmployerControllerTests
{
    private readonly Mock<ICosmosDbService> _mockCosmosDbService;
    private readonly EmployerController _controller;

    public EmployerControllerTests()
    {
        _mockCosmosDbService = new Mock<ICosmosDbService>();
        _controller = new EmployerController(_mockCosmosDbService.Object);
    }

    [Fact]
    public async Task AddQuestion_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var questionDto = new QuestionDto
        {
            Content = "What is your name?",
            Type = "Text",
            Options = new List<string>(),
            MinValue = 0,
            MaxValue = 0,
            Placeholder = "Enter your name"
        };

        // Act
        var result = await _controller.AddQuestion(questionDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnQuestion = Assert.IsType<Question>(createdResult.Value);
        Assert.Equal(questionDto.Content, returnQuestion.Content);
    }

    [Fact]
    public async Task GetQuestion_ReturnsNotFound_WhenQuestionDoesNotExist()
    {
        // Arrange
        _mockCosmosDbService.Setup(s => s.GetQuestionAsync(It.IsAny<string>()))
                            .ReturnsAsync((Question)null);

        // Act
        var result = await _controller.GetQuestion("nonexistent-id");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetQuestion_ReturnsOkResult_WithQuestion()
    {
        // Arrange
        var question = new Question
        {
            Id = "1",
            Content = "What is your name?",
            Type = "Text",
            Options = new List<string>(),
            MinValue = 0,
            MaxValue = 0,
            Placeholder = "Enter your name"
        };

        _mockCosmosDbService.Setup(s => s.GetQuestionAsync(It.IsAny<string>()))
                            .ReturnsAsync(question);

        // Act
        var result = await _controller.GetQuestion("1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnQuestion = Assert.IsType<Question>(okResult.Value);
        Assert.Equal(question.Content, returnQuestion.Content);
    }

    [Fact]
    public async Task UpdateQuestion_ReturnsNotFound_WhenQuestionDoesNotExist()
    {
        // Arrange
        _mockCosmosDbService.Setup(s => s.GetQuestionAsync(It.IsAny<string>()))
                            .ReturnsAsync((Question)null);

        var questionDto = new QuestionDto
        {
            Content = "Updated content",
            Type = "Text",
            Options = new List<string>(),
            MinValue = 0,
            MaxValue = 0,
            Placeholder = "Enter updated content"
        };

        // Act
        var result = await _controller.UpdateQuestion("nonexistent-id", questionDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateQuestion_ReturnsNoContent_WhenQuestionIsUpdated()
    {
        // Arrange
        var question = new Question
        {
            Id = "1",
            Content = "What is your name?",
            Type = "Text",
            Options = new List<string>(),
            MinValue = 0,
            MaxValue = 0,
            Placeholder = "Enter your name"
        };

        _mockCosmosDbService.Setup(s => s.GetQuestionAsync(It.IsAny<string>()))
                            .ReturnsAsync(question);

        var questionDto = new QuestionDto
        {
            Content = "Updated content",
            Type = "Text",
            Options = new List<string>(),
            MinValue = 0,
            MaxValue = 0,
            Placeholder = "Enter updated content"
        };

        // Act
        var result = await _controller.UpdateQuestion("1", questionDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteQuestion_ReturnsNotFound_WhenQuestionDoesNotExist()
    {
        // Arrange
        _mockCosmosDbService.Setup(s => s.GetQuestionAsync(It.IsAny<string>()))
                            .ReturnsAsync((Question)null);

        // Act
        var result = await _controller.DeleteQuestion("nonexistent-id");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteQuestion_ReturnsNoContent_WhenQuestionIsDeleted()
    {
        // Arrange
        var question = new Question
        {
            Id = "1",
            Content = "What is your name?",
            Type = "Text",
            Options = new List<string>(),
            MinValue = 0,
            MaxValue = 0,
            Placeholder = "Enter your name"
        };

        _mockCosmosDbService.Setup(s => s.GetQuestionAsync(It.IsAny<string>()))
                            .ReturnsAsync(question);

        // Act
        var result = await _controller.DeleteQuestion("1");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
