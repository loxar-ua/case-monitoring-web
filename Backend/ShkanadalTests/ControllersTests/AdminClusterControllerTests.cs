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
using System.Threading.Tasks;

namespace ShkanadalTests.ControllersTests
{
    public class AdminClusterControllerTests
    {
        private readonly Mock<IAdminClusterService> _serviceMock;
        private readonly AdminClustersController _controller;

        public AdminClusterControllerTests()
        {
            _serviceMock = new Mock<IAdminClusterService>();
            _controller = new AdminClustersController(_serviceMock.Object);
        }

        ////GetAllClustersAsync

        [Fact]
        public async Task GetAllClustersAsync_ReturnsEmpty()
        {
            //Arrange
            var clusters = new List<ClusterAdminReadDto> { };
            var pagedList = PagedList<ClusterAdminReadDto>.Create(clusters.AsQueryable(), 1, 10);

            _serviceMock.Setup(s => s.GetAllClustersAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(pagedList);

            //Act
            var result = await _controller.GetAllClustersAsync(null, 1, 10);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<PagedList<ClusterAdminReadDto>>(okResult.Value);
            Assert.Equal(0, returnedList.TotalCount);
        }

        [Fact]
        public async Task GetAllClustersAsync_ReturnsClusters()
        {
            //Arrange
            var clusters = new List<ClusterAdminReadDto> {
                new ClusterAdminReadDto { Id = 6, Name = "Name", IsActive = true, Content = "Content", FeaturedImageURL = "FeaturedImageURL", LastUpdatedAt = new DateTime(2025, 12, 8) },
                new ClusterAdminReadDto { Id = 8, Name = "Name1", IsActive = false, Content = "Content1", FeaturedImageURL = "FeaturedImageURL1", LastUpdatedAt = new DateTime(2025, 7, 6) }
            };
            var pagedList = PagedList<ClusterAdminReadDto>.Create(clusters.AsQueryable(), 1, 10);

            _serviceMock.Setup(s => s.GetAllClustersAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(pagedList);

            //Act
            var result = await _controller.GetAllClustersAsync(null, 1, 10);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<PagedList<ClusterAdminReadDto>>(okResult.Value);

            Assert.Equal(2, returnedList.TotalCount);
            Assert.Equal("Name", returnedList.Items[0].Name);
            Assert.Equal("Name1", returnedList.Items[1].Name);
            Assert.True(returnedList.Items[0].IsActive);
            Assert.False(returnedList.Items[1].IsActive);
            Assert.Equal("Content", returnedList.Items[0].Content);
            Assert.Equal("Content1", returnedList.Items[1].Content);
        }

        //GetClusterById

        [Fact]
        public async Task GetClusterById_ReturnsNotFound()
        {
            //Arrange
            _serviceMock.Setup(s => s.GetClusterByIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((ClusterAdminDetailedReadDto?)null);

            // Act
            var result = await _controller.GetClusterByIdAsync(7);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetClusterById_ReturnsOk()
        {
            //Arrange
            var clusterDto = new ClusterAdminDetailedReadDto
            {
                Id = 7,
                Name = "Name",
                IsActive = true,
                Content = "Some content",
                FeaturedImageURL = "Image",
                LastUpdatedAt = new DateTime(2025, 12, 10),
                Articles = new List<ArticleReadDto>()
            };

            _serviceMock.Setup(s => s.GetClusterByIdAsync(7))
                        .ReturnsAsync(clusterDto);

            // Act
            var result = await _controller.GetClusterByIdAsync(7);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCluster = Assert.IsType<ClusterAdminDetailedReadDto>(okResult.Value);
            Assert.Equal(7, returnedCluster.Id);
            Assert.Equal("Name", returnedCluster.Name);
        }

        //UpdateAsync

        [Fact]
        public async Task UpdateAsync_ReturnsNotFound()
        {
            //Arrange
            var request = new ClusterAdminUpdateRequest
            {
                Name = "Name",
                IsActive = true,
                Content = "Some content",
                FeaturedImageURL = "FeaturedImageURL"
            };

            _serviceMock.Setup(s => s.UpdateAsync(7, It.IsAny<ClusterAdminUpdateRequest>()))
                        .ReturnsAsync((ClusterAdminUpdateDto?)null);

            //Act
            var result = await _controller.UpdateAsync(7, request);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsOk()
        {
            //Arrange
            var clusterDto = new ClusterAdminUpdateDto
            {
                Name = "UpdatedName",
                IsActive = false,
                Content = "Updated content",
                FeaturedImageURL = "UpdatedURL"
            };

            var request = new ClusterAdminUpdateRequest
            {
                Name = "RequestName",
                IsActive = true,
                Content = "Some content",
                FeaturedImageURL = "RequestURL"
            };

            _serviceMock.Setup(s => s.UpdateAsync(7, request))
                        .ReturnsAsync(clusterDto);

            //Act
            var result = await _controller.UpdateAsync(7, request);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCluster = Assert.IsType<ClusterAdminUpdateDto>(okResult.Value);

            Assert.Equal("UpdatedName", returnedCluster.Name);
            Assert.Equal("UpdatedURL", returnedCluster.FeaturedImageURL);
            _serviceMock.Verify(s => s.UpdateAsync(7, request), Times.Once);
        }

    }
}
