using AutoMapper;
using Moq;
using shkandalData.DTOs.ArticleDtos;
using shkandalData.DTOs.ClusterDtos;
using shkandalData.DTOs.MediaDtos;
using ShkandalData.Common;
using ShkandalData.Models;
using ShkandalInfrastructure.Repositories;
using ShkandalServices;
using System.Diagnostics.Metrics;

namespace ShkanadalTests.ServicesTests
{
    public class AdminArticleServiceTests
    {
        private readonly Mock<IAdminArticlesRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AdminArticleService _service;

        public AdminArticleServiceTests()
        {
            _repositoryMock = new Mock<IAdminArticlesRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new AdminArticleService(_repositoryMock.Object, _mapperMock.Object);
        }

        Article article = new Article
        {
            Id = 7,
            ClusterId = 5,
            MediaId = 3,
            Media = new Media { Id = 3, Name = "Media", SitemapIndexURL = "URL" },
            Cluster = new Cluster { Id = 5, Name = "Cluster", ViewCounter = 0 },
            Title = "Title",
            Link = "Link",
            FeaturedImageURL = "FeaturedImageURL",
            Author = "Author",
            Content = "Some content",
            Status = "Active",
            PublishedAt = new DateTime(2025, 12, 10),
            IsRelevant = true,
            IsChecked = true
        };

        //GetArticleByIdAsync
        [Fact]
        public async Task GetArticleByIdAsync_ReturnsNull()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetArticleByIdAsync(7))
                           .ReturnsAsync((Article?)null);

            //Act
            var result = await _service.GetArticleByIdAsync(7);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetArticleByIdAsync_ReturnsArticle()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetArticleByIdAsync(7))
                           .ReturnsAsync(article);

            _mapperMock.Setup(m => m.Map<ArticleAdminReadDto>(article))
                       .Returns(new ArticleAdminReadDto
                       {
                           Id = article.Id,
                           Media = new MediaReadDto { Name = article.Media.Name },
                           Cluster = new ClusterReadDto { Id = article.Cluster.Id, Name = article.Cluster.Name },
                           Title = article.Title,
                           Link = article.Link,
                           FeaturedImageURL = article.FeaturedImageURL,
                           Author = article.Author,
                           Content = article.Content,
                           PublishedAt = article.PublishedAt.Value,
                           IsRelevant = article.IsRelevant
                       });

            //Act
            var result = await _service.GetArticleByIdAsync(7);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal(5, result.Cluster.Id);
            Assert.Equal("Media", result.Media.Name);
            Assert.Equal("Title", result.Title);
            Assert.Equal("Link", result.Link);
            Assert.Equal("FeaturedImageURL", result.FeaturedImageURL);
            Assert.Equal("Author", result.Author);
            Assert.Equal("Some content", result.Content);
            Assert.Equal(new DateTime(2025, 12, 10), result.PublishedAt);
            Assert.True(result.IsRelevant);
        }


        //GetAllArticlesNotCheckedAsync
        [Fact]
        public async Task GetAllArticlesNotCheckedAsync_ReturnsArticles()
        {
            //Arrange
            var articles = new List<Article>
            {
                article,
                new Article {
            Id = 8,
            ClusterId = 4,
            MediaId = 2,
            Media = new Media { Id = 2, Name = "Media1", SitemapIndexURL = "URL1"},
            Cluster = new Cluster { Id = 4, Name = "Cluster1", ViewCounter = 10 },
            Title = "Title1",
            Link = "Link1",
            FeaturedImageURL = "FeaturedImageURL1",
            Author = "Author1",
            Content = "Some content1",
            Status = "Not Active",
            PublishedAt = new DateTime(2025, 8, 10),
            IsRelevant = false,
            IsChecked = false,
        }
            };

            var pagedClusters =  PagedList<Article>.Create(articles.AsQueryable(), 1, 10);

            _repositoryMock.Setup(r => r.GetAllArticlesNotCheckedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(pagedClusters);

            _mapperMock.Setup(m => m.Map<ArticleAdminReadDto>(It.IsAny<Article>()))
             .Returns((Article src) => new ArticleAdminReadDto
             {
                 Id = src.Id,
                 Media = new MediaReadDto { Name = src.Media.Name },
                 Cluster = new ClusterReadDto { Id = src.Cluster.Id, Name = src.Cluster.Name },
                 Title = src.Title,
                 Link = src.Link,
                 FeaturedImageURL = src.FeaturedImageURL,
                 Author = src.Author,
                 Content = src.Content,
                 PublishedAt = src.PublishedAt.Value,
                 IsRelevant = src.IsRelevant
             });

            //Act
            var result = await _service.GetAllArticlesNotCheckedAsync(null, 1, 10);

            //Assert
            Assert.NotNull(result);

            Assert.Equal(7, result.Items[0].Id);
            Assert.Equal(5, result.Items[0].Cluster.Id);
            Assert.Equal("Media", result.Items[0].Media.Name);
            Assert.Equal("Title", result.Items[0].Title);
            Assert.Equal("Link", result.Items[0].Link);
            Assert.Equal("FeaturedImageURL", result.Items[0].FeaturedImageURL);
            Assert.Equal("Author", result.Items[0].Author);
            Assert.Equal("Some content", result.Items[0].Content);
            Assert.Equal(new DateTime(2025, 12, 10), result.Items[0].PublishedAt);
            Assert.True(result.Items[0].IsRelevant);

            Assert.Equal(8, result.Items[1].Id);
            Assert.Equal(4, result.Items[1].Cluster.Id);
            Assert.Equal("Media1", result.Items[1].Media.Name);
            Assert.Equal("Title1", result.Items[1].Title);
            Assert.Equal("Link1", result.Items[1].Link);
            Assert.Equal("FeaturedImageURL1", result.Items[1].FeaturedImageURL);
            Assert.Equal("Author1", result.Items[1].Author);
            Assert.Equal("Some content1", result.Items[1].Content);
            Assert.Equal(new DateTime(2025, 8, 10), result.Items[1].PublishedAt);
            Assert.False(result.Items[1].IsRelevant);

        }

        [Fact]
        public async Task GetAllArticlesNotCheckedAsync_ReturnsEmpty()
        {
            //Arrange
            var articles = new List<Article>();
            var emptyPaged =  PagedList<Article>.Create(articles.AsQueryable(), 1, 10);

            _repositoryMock.Setup(r => r.GetAllArticlesNotCheckedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(emptyPaged);

            //Act
            var result = await _service.GetAllArticlesNotCheckedAsync(null, 1, 10);

            //Assert
            Assert.Empty(result.Items);
        }


        //ArticleUpdate
        [Fact]
        public async Task UpdateAsync_ReturnsNull()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetArticleByIdAsync(7))
                           .ReturnsAsync((Article?)null);

            var request = new ArticleAdminUpdateRequest();

            // Act
            var result = await _service.UpdateAsync(7, request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProvidedFields()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetArticleByIdAsync(7))
                            .ReturnsAsync(article);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Article>()))
                           .ReturnsAsync((Article a) => a);

            _repositoryMock.Setup(r => r.ClusterExistsAsync(10))
                         .ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map<ArticleAdminUpdateDto>(It.IsAny<Article>()))
                       .Returns((Article a) => new ArticleAdminUpdateDto
                       {
                           IsChecked = a.IsChecked,
                           ClusterId = a.ClusterId
                       });


            var request = new ArticleAdminUpdateRequest
            {
                IsChecked = false,
                ClusterId = 10,
                DetachCluster = false
            };

            // Act
            var result = await _service.UpdateAsync(7, request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsChecked);
            Assert.Equal(10, result.ClusterId);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Article>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DoesNotOverride()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetArticleByIdAsync(7))
                           .ReturnsAsync(article);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Article>()))
                           .ReturnsAsync((Article a) => a);

            _mapperMock.Setup(m => m.Map<ArticleAdminUpdateDto>(It.IsAny<Article>()))
                       .Returns((Article a) => new ArticleAdminUpdateDto
                       {
                           IsChecked = a.IsChecked,
                           ClusterId = a.ClusterId
                       });

            var request = new ArticleAdminUpdateRequest
            {
                IsChecked = null,
                ClusterId = null,
                DetachCluster = false
            };

            // Act
            var result = await _service.UpdateAsync(7, request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsChecked);
            Assert.Equal(5, result.ClusterId);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Article>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DetachesCluster()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetArticleByIdAsync(7))
                           .ReturnsAsync(article);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Article>()))
                           .ReturnsAsync((Article a) => a);

            _mapperMock.Setup(m => m.Map<ArticleAdminUpdateDto>(It.IsAny<Article>()))
                       .Returns((Article a) => new ArticleAdminUpdateDto
                       {
                           IsChecked = a.IsChecked,
                           ClusterId = a.ClusterId
                       });

            var request = new ArticleAdminUpdateRequest
            {
                IsChecked = null,
                ClusterId = null,
                DetachCluster = true
            };

            // Act
            var result = await _service.UpdateAsync(7, request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsChecked);
            Assert.Null(result.ClusterId);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Article>()), Times.Once);
        }
    }
}

