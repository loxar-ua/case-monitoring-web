using AutoMapper;
using Moq;
using ShkandalServices;
using ShkandalInfrastructure.Repositories;
using ShkandalData.DTOs.EventDtos;
using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using shkandalData.DTOs.ArticleDtos;
using shkandalData.DTOs.MediaDtos;

namespace ShkanadalTests.ServicesTests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly EventService _service;

        public EventServiceTests()
        {
            _repositoryMock = new Mock<IEventRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new EventService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByIdAsync(1))
                           .ReturnsAsync((Event?)null);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedDto_WhenFound()
        {
            // Arrange
            var evt = new Event
            {
                Id = 2,
                Title = "Test Event",
                Description = "Desc",
                Date = new DateTime(2025, 6, 1),
                Articles = new List<Article>
                {
                    new Article
                    {
                        Id = 10,
                        Title = "Article 1",
                        MediaId = 1,
                        Media = new Media { Id = 1, Name = "Media1", SitemapIndexURL = "url" },
                        IsRelevant = true,
                        IsChecked = true
                    }
                }
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(2))
                           .ReturnsAsync(evt);

            var dto = new EventWithArticlesReadDto
            {
                Id = evt.Id,
                Title = evt.Title,
                Description = evt.Description,

                Articles = evt.Articles
                    .Select(a => new ArticleReadDto
                    {
                        Id = a.Id,
                        Media = a.Media != null ? new MediaReadDto { Name = a.Media.Name } : new MediaReadDto { Name = string.Empty },
                        Title = a.Title ?? string.Empty,
                        Link = a.Link ?? string.Empty,
                        FeaturedImageURL = a.FeaturedImageURL ?? string.Empty,
                        Author = a.Author ?? string.Empty,
                        Content = a.Content ?? string.Empty,
                        PublishedAt = a.PublishedAt ?? DateTime.MinValue,
                        IsChecked = a.IsChecked
                    })
                    .ToList()
            };

            _mapperMock.Setup(m => m.Map<EventWithArticlesReadDto>(evt))
                       .Returns(dto);

            // Act
            var result = await _service.GetByIdAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("Test Event", result.Title);
            Assert.Equal(new DateTime(2025, 6, 1), result.Date);
            Assert.Single(result.Articles);
            Assert.Equal(10, result.Articles.First().Id);
        }
    }
}