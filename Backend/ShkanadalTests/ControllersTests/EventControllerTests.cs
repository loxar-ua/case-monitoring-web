using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ShkandalServices;
using ShkandalData.DTOs.EventDtos;
using shkandal_api.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using shkandalData.DTOs.ArticleDtos;

namespace ShkanadalTests.ControllersTests
{
    public class EventControllerTests
    {
        private readonly Mock<IEventService> _serviceMock;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _serviceMock = new Mock<IEventService>();
            _controller = new EventController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenServiceReturnsNull()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(1))
                        .ReturnsAsync((EventWithArticlesReadDto?)null);

            // Act
            var actionResult = await _controller.GetByIdAsync(1);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkWithDto_WhenFound()
        {
            // Arrange
            var dto = new EventWithArticlesReadDto
            {
                Id = 5,
                Title = "Event Title",
                Description = "Desc",
                Articles = new List<ArticleReadDto>() 
            };

            _serviceMock.Setup(s => s.GetByIdAsync(5))
                        .ReturnsAsync(dto);

            // Act
            var actionResult = await _controller.GetByIdAsync(5);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Same(dto, okResult.Value);
        }
    }
}