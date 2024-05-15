using WebApplicationForm.Controllers;
using WebApplicationForm.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using WebApplicationForm;

public class ApplicationDetailsControllerTests
{
    private readonly Mock<IApplicationDetailsService> _mockService;
    private readonly ApplicationDetailsController _controller;

    public ApplicationDetailsControllerTests()
    {
        _mockService = new Mock<IApplicationDetailsService>();
        _controller = new ApplicationDetailsController(_mockService.Object);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var details = new ApplicationDetails { TenantId = "10", PersonalInformation = new PersonalInformation() };
        var createdDetails = new ApplicationDetails { Id = "1", TenantId = "10" };
        _mockService.Setup(s => s.CreateAsync(It.IsAny<ApplicationDetails>()))
                    .ReturnsAsync(createdDetails);

        // Act
        var result = await _controller.Create(details);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnDetails = Assert.IsType<ApplicationDetails>(createdResult.Value);
        Assert.Equal("1", returnDetails.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenDetailsDoNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync((ApplicationDetails)null);

        // Act
        var result = await _controller.GetById("1", "10");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WithDetails()
    {
        // Arrange
        var details = new ApplicationDetails { Id = "1", TenantId = "10", PersonalInformation = new PersonalInformation() };
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(details);

        // Act
        var result = await _controller.GetById("1", "10");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnDetails = Assert.IsType<ApplicationDetails>(okResult.Value);
        Assert.Equal("1", returnDetails.Id);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenDetailsDoNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync((ApplicationDetails)null);

        // Act
        var result = await _controller.Update("1", new ApplicationDetails(), "10");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenDetailsExist()
    {
        // Arrange
        var details = new ApplicationDetails { Id = "1", TenantId = "10", PersonalInformation = new PersonalInformation() };
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(details);

        // Act
        var result = await _controller.Update("1", details, "10");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenDetailsDoNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync((ApplicationDetails)null);

        // Act
        var result = await _controller.Delete("1", "10");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDetailsExist()
    {
        // Arrange
        var details = new ApplicationDetails { Id = "1", TenantId = "10", PersonalInformation = new PersonalInformation() };
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(details);

        // Act
        var result = await _controller.Delete("1", "10");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
