using Microsoft.AspNetCore.Mvc;
using Moq;
using shkandal_api.Controllers;
using shkandalData.DTOs.ArticleDtos;
using shkandalData.DTOs.ClusterDtos;
using ShkandalData.Common;
using ShkandalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkanadalTests.ControllersTests
{
    public class ClusterControllerTests
    {
        private readonly Mock<IClusterService> _serviceMock;
        private readonly ClusterController _controller;

        public ClusterControllerTests()
        {
            _serviceMock = new Mock<IClusterService>();
            _controller = new ClusterController(_serviceMock.Object);
        }


        //GetAllAsync

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty()
        {
            //Arrange
            var clusters = new List<ClusterReadDto> { };

            var pagedList = PagedList<ClusterReadDto>.Create(clusters.AsQueryable(), 1, 10);

            _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(pagedList);

            //Act
            var result = await _controller.GetAllAsync(null, 1, 10);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<PagedList<ClusterReadDto>>(okResult.Value);
            Assert.Equal(0, returnedList.TotalCount);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsClusters()
        {
            //Arrange
            var clusters = new List<ClusterReadDto> {
                new ClusterReadDto { Id = 6, Name = "Name", Content = "Content", FeaturedImageURL = "FeaturedImageURL", LastUpdatedAt = new DateTime(2025, 12, 8) },
                new ClusterReadDto { Id = 8, Name = "Name1", Content = "Content1", FeaturedImageURL = "FeaturedImageURL1", LastUpdatedAt = new DateTime(2025, 7, 6) }
            };

            var pagedList = PagedList<ClusterReadDto>.Create(clusters.AsQueryable(), 1, 10);

            _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(pagedList);

            //Act
            var result = await _controller.GetAllAsync(null, 1, 10);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<PagedList<ClusterReadDto>>(okResult.Value);
            Assert.Equal(2, returnedList.TotalCount);
            Assert.Equal("Name", returnedList.Items[0].Name);
            Assert.Equal("Name1", returnedList.Items[1].Name);
            Assert.Equal("Content", returnedList.Items[0].Content);
            Assert.Equal("Content1", returnedList.Items[1].Content);
        }


        //GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((ClusterDetailedReadDto?)null);

            // Act
            var result = await _controller.GetByIdAsync(7);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOk()
        {
            //Arrange
            var clusterDto = new ClusterDetailedReadDto
            {
                Id = 7,
                Name = "Name",
                ViewCounter = 7,
                Content = "Some content",
                FeaturedImageURL = "FeaturedImageURL",
                Articles = new List<ArticleReadDto>()
            };

            _serviceMock.Setup(s => s.GetByIdAsync(7))
                       .ReturnsAsync(clusterDto);

            // Act
            var result = await _controller.GetByIdAsync(7);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCluster = Assert.IsType<ClusterDetailedReadDto>(okResult.Value);
            Assert.Equal(7, returnedCluster.Id);
            Assert.Equal("Name", returnedCluster.Name);
        }


        //IncrementViewCounter

        [Fact]
        public async Task IncrementViewCounter_ReturnsNotFound()
        {
            //Arrange
            _serviceMock.Setup(s => s.IncrementViewCounterAsync(7))
                .ReturnsAsync((ClusterUpdateDto?)null);

            //Act
            var result = await _controller.IncrementViewCounter(7);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task IncrementViewCounter_ReturnsOk()
        {
            //Arrange
            var clusterDto = new ClusterUpdateDto
            {
                Id = 7,
                ViewCounter = 7
            };

            _serviceMock.Setup(s => s.IncrementViewCounterAsync(7))
                .ReturnsAsync(clusterDto);

            //Act
            var result = await _controller.IncrementViewCounter(7);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCluster = Assert.IsType<ClusterUpdateDto>(okResult.Value);
            Assert.Equal(7, returnedCluster.Id);
            Assert.Equal(7, returnedCluster.ViewCounter);
        }
    }
}
