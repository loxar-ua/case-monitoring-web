using AutoMapper;
using Moq;
using shkandalData.DTOs.ArticleDtos;
using shkandalData.DTOs.ClusterDtos;
using ShkandalData.Common;
using ShkandalData.Models;
using ShkandalInfrastructure.Repositories;
using ShkandalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkanadalTests.ServicesTests
{
    public class AdminClusterServiceTests
    {
        private readonly Mock<IAdminClustersRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AdminClusterService _service;

        public AdminClusterServiceTests()
        {
            _repositoryMock = new Mock<IAdminClustersRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new AdminClusterService(_repositoryMock.Object, _mapperMock.Object);
        }

        Cluster cluster = new()
        {
            Id = 7,
            Name = "Test Cluster",
            IsRelevant = true,
            ViewCounter = 1,
            Summary = "Some content",
            FeaturedImageURL = "FeaturedImageURL",
            LastUpdatedAt = new DateTime(2025, 12, 2),
            Articles = new List<Article>()
        };

        //GetClusterById
        [Fact]
        public async Task GetClusterById_ReturnsNull_WhenNotFound()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetClusterByIdAsync(7))
                           .ReturnsAsync((Cluster?)null);

            //Act
            var result = await _service.GetClusterByIdAsync(7);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetClusterById_ReturnsAdminClusterDto()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetClusterByIdAsync(7))
                           .ReturnsAsync(cluster);

            _mapperMock.Setup(m => m.Map<ClusterAdminDetailedReadDto>(cluster))
                       .Returns(new ClusterAdminDetailedReadDto
                       {
                           Id = cluster.Id,
                           Name = cluster.Name,
                           IsRelevant = cluster.IsRelevant,
                           Summary = cluster.Summary,
                           LastUpdatedAt = cluster.LastUpdatedAt,
                           FeaturedImageURL = cluster.FeaturedImageURL,
                           Articles = new List<ArticleReadDto>()
                       });

            //Act
            var result = await _service.GetClusterByIdAsync(7);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("Test Cluster", result.Name);
            Assert.True(result.IsRelevant);
            Assert.Equal("Some content", result.Summary);
            Assert.Equal(new DateTime(2025, 12, 2), result.LastUpdatedAt);
            Assert.Equal("FeaturedImageURL", result.FeaturedImageURL);
            Assert.Empty(result.Articles);
        }

        //UpdateAsync
        [Fact]
        public async Task UpdateAsync_ReturnsNull()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetClusterByIdAsync(7))
                           .ReturnsAsync((Cluster?)null);

            //Act
            var request = new ClusterAdminUpdateRequest();
            var result = await _service.UpdateAsync(7, request);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProvidedFields()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetClusterByIdAsync(7))
                           .ReturnsAsync(cluster);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Cluster>()))
                           .ReturnsAsync((Cluster c) => c);

            _mapperMock.Setup(m => m.Map<ClusterAdminUpdateDto>(It.IsAny<Cluster>()))
                       .Returns((Cluster c) => new ClusterAdminUpdateDto
                       {
                           Name = c.Name,
                           IsRelevant = c.IsRelevant,
                           Summary = c.Summary,
                           FeaturedImageURL = c.FeaturedImageURL
                       });

            var request = new ClusterAdminUpdateRequest
            {
                Name = "newName",
                Summary = "newContent",
                FeaturedImageURL = "newImage",
                IsRelevant = false
            };

            //Act
            var result = await _service.UpdateAsync(7, request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("newName", result!.Name);
            Assert.Equal("newContent", result.Summary);
            Assert.Equal("newImage", result.FeaturedImageURL);
            Assert.False(result.IsRelevant);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Cluster>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DoesNotOverride()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetClusterByIdAsync(7))
                           .ReturnsAsync(cluster);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Cluster>()))
                           .ReturnsAsync((Cluster c) => c);

            _mapperMock.Setup(m => m.Map<ClusterAdminUpdateDto>(It.IsAny<Cluster>()))
                       .Returns((Cluster c) => new ClusterAdminUpdateDto
                       {
                           Name = c.Name,
                           IsRelevant = c.IsRelevant,
                           Summary = c.Summary,
                           FeaturedImageURL = c.FeaturedImageURL
                       });

            var request = new ClusterAdminUpdateRequest
            {
                Name = null,
                Summary = "",
                FeaturedImageURL = "   ",
                IsRelevant = false
            };

            //Act
            var result = await _service.UpdateAsync(7, request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test Cluster", result!.Name);
            Assert.Equal("Some content", result.Summary);
            Assert.Equal("FeaturedImageURL", result.FeaturedImageURL);
            Assert.False(result.IsRelevant);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Cluster>()), Times.Once);
        }

        //GetAllClusters
        [Fact]
        public async Task GetAllClustersAsync_ReturnsClusters()
        {
            //Arrange
            var clusters = new List<Cluster>
            {
                cluster,
                new Cluster
                {
                    Id = 9,
                    Name = "Test Cluster1",
                    IsRelevant = false,
                    ViewCounter = 60,
                    Summary = "Some other content",
                    FeaturedImageURL = "FeaturedImageURL1",
                    LastUpdatedAt = new DateTime(2025, 10, 2),
                    Articles = new List<Article>()
                }
            };

            var pagedClusters = PagedList<Cluster>.Create(clusters.AsQueryable(), 1, 10);

            _repositoryMock.Setup(r => r.GetAllClustersAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(pagedClusters);

            _mapperMock.Setup(m => m.Map<ClusterAdminReadDto>(It.IsAny<Cluster>()))
                       .Returns((Cluster src) => new ClusterAdminReadDto
                       {
                           Id = src.Id,
                           Name = src.Name,
                           IsRelevant = src.IsRelevant,
                           Summary = src.Summary,
                           FeaturedImageURL = src.FeaturedImageURL,
                           LastUpdatedAt = src.LastUpdatedAt,
                       });

            //Act
            var result = await _service.GetAllClustersAsync(null, 1, 10);

            //Assert
            Assert.NotEmpty(result.Items);
            Assert.Equal(7, result.Items[0].Id);
            Assert.Equal("Test Cluster", result.Items[0].Name);
            Assert.True(result.Items[0].IsRelevant);
            Assert.Equal("Some content", result.Items[0].Summary);
            Assert.Equal("FeaturedImageURL", result.Items[0].FeaturedImageURL);
            Assert.Equal(new DateTime(2025, 12, 2), result.Items[0].LastUpdatedAt);

            Assert.Equal(9, result.Items[1].Id);
            Assert.Equal("Test Cluster1", result.Items[1].Name);
            Assert.False(result.Items[1].IsRelevant);
            Assert.Equal("Some other content", result.Items[1].Summary);
            Assert.Equal("FeaturedImageURL1", result.Items[1].FeaturedImageURL);
            Assert.Equal(new DateTime(2025, 10, 2), result.Items[1].LastUpdatedAt);
        }

        [Fact]
        public async Task GetAllClustersAsync_ReturnsEmpty()
        {
            //Arrange
            var clusters = new List<Cluster>();
            var emptyPaged = PagedList<Cluster>.Create(clusters.AsQueryable(), 1, 10);

            _repositoryMock.Setup(r => r.GetAllClustersAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(emptyPaged);

            //Act
            var result = await _service.GetAllClustersAsync(null, 1, 10);

            //Assert
            Assert.Empty(result.Items);
        }
    }
}
