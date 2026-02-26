using AutoMapper;
using Moq;
using ShkandalServices;
using shkandalData.DTOs.ClusterDtos;
using shkandalData.DTOs.ArticleDtos;
using ShkandalData.Models;
using ShkandalInfrastructure.Repositories;
using ShkandalData.Common;

namespace ShkanadalTests.ServicesTests
{
    public class ClusterServiceTests
    {
        private readonly Mock<IClusterRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly ClusterService _service;

        public ClusterServiceTests()
        {
            _repositoryMock = new Mock<IClusterRepository>();
            _mapperMock = new Mock<IMapper>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _service = new ClusterService(
            _repositoryMock.Object,
            _categoryRepositoryMock.Object, 
            _mapperMock.Object
        );
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


        // GetByIdAsync 

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByIdAsync(7))
                           .ReturnsAsync((Cluster?)null);

            // Act
            var result = await _service.GetByIdAsync(7);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsClusterDto()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByIdAsync(7))
                           .ReturnsAsync(cluster);

            _mapperMock.Setup(m => m.Map<ClusterDetailedReadDto>(cluster))
                       .Returns(new ClusterDetailedReadDto
                       {
                           Id = cluster.Id,
                           Name = cluster.Name,
                           ViewCounter = cluster.ViewCounter,
                           Summary = cluster.Summary,
                           FeaturedImageURL = cluster.FeaturedImageURL,
                           Articles = new List<ArticleReadDto>()
                       });

            // Act
            var result = await _service.GetByIdAsync(7);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("Test Cluster", result.Name);
            Assert.Equal(1, result.ViewCounter);
            Assert.Equal("Some content", result.Summary);
            Assert.Equal("FeaturedImageURL", result.FeaturedImageURL);
            Assert.Empty(result.Articles);
        }


        // IncrementViewCounterAsync 

        [Fact]
        public async Task IncrementViewCounterAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _repositoryMock.Setup(r => r.IncrementViewCounterAsync(7))
                           .ReturnsAsync((Cluster?)null);

            // Act
            var result = await _service.IncrementViewCounterAsync(7);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task IncrementViewCounterAsync_ReturnsClusterDto()
        {
            // Arrange
            _repositoryMock.Setup(r => r.IncrementViewCounterAsync(7))
                           .ReturnsAsync(cluster);

            _mapperMock.Setup(m => m.Map<ClusterUpdateDto>(cluster))
                       .Returns(new ClusterUpdateDto
                       {
                           Id = cluster.Id,
                           ViewCounter = cluster.ViewCounter
                       });

            // Act
            var result = await _service.IncrementViewCounterAsync(7);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal(1, result.ViewCounter);
        }


        //GetAllAsync 

        [Fact]
        public async Task GetAllAsync_ReturnsClusters()
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

            _repositoryMock.Setup(r => r.GetAllAsync(
                It.IsAny<string?>(), 
                It.IsAny<int?>(),    
                It.IsAny<string?>(), 
                It.IsAny<int>(),     
                It.IsAny<int>()      
                ))
               .ReturnsAsync(pagedClusters);

            _mapperMock.Setup(m => m.Map<ClusterReadDto>(It.IsAny<Cluster>()))
             .Returns((Cluster src) => new ClusterReadDto
             {
                 Id = src.Id,
                 Name = src.Name,
                 Summary = src.Summary,
                 FeaturedImageURL = src.FeaturedImageURL,
                 LastUpdatedAt = src.LastUpdatedAt,
             });

            //Act
            var result = await _service.GetAllAsync(null, null, null, 1, 10);

            //Assert
            Assert.NotEmpty(result.Items);

            Assert.Equal(7, result.Items[0].Id);
            Assert.Equal("Test Cluster", result.Items[0].Name);
            Assert.Equal("Some content", result.Items[0].Summary);
            Assert.Equal("FeaturedImageURL", result.Items[0].FeaturedImageURL);
            Assert.Equal(new DateTime(2025, 12, 2), result.Items[0].LastUpdatedAt);

            Assert.Equal(9, result.Items[1].Id);
            Assert.Equal("Test Cluster1", result.Items[1].Name);
            Assert.Equal("Some other content", result.Items[1].Summary);
            Assert.Equal("FeaturedImageURL1", result.Items[1].FeaturedImageURL);
            Assert.Equal(new DateTime(2025, 10, 2), result.Items[1].LastUpdatedAt);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty()
        {
            //Arrange
            var clusters = new List<Cluster> { };
            var emptyPaged =  PagedList<Cluster>.Create(clusters.AsQueryable(), 1, 10);
            _repositoryMock.Setup(r => r.GetAllAsync(
                It.IsAny<string?>(),
                It.IsAny<int?>(),    
                It.IsAny<string?>(), 
                It.IsAny<int>(),
                It.IsAny<int>()
                ))
                .ReturnsAsync(emptyPaged);

            //Act
            var result = await _service.GetAllAsync(null, null, null, 1, 10);

            //Assert
            Assert.Equal(0, result.TotalCount);
        }
    }
}
