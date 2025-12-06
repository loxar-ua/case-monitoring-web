using Microsoft.AspNetCore.Mvc;
using Moq;
using shkandal_api.Controllers;
using shkandalData.DTOs.ArticleDtos;
using ShkandalData.Common;
using ShkandalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShkanadalTests.ControllersTests
{
    public class AdminArticleControllerTests
    {
        private readonly Mock<IAdminArticleService> _serviceMock;
        private readonly AdminArticlesController _controller;

        public AdminArticleControllerTests()
        {
            _serviceMock = new Mock<IAdminArticleService>();
            _controller = new AdminArticlesController(_serviceMock.Object);
        }

        //GetAllArticlesNotCheckedAsync

        [Fact]
        public async Task GetAllArticlesNotCheckedAsync_ReturnsEmpty()
        {
            //Arrange
            var articles = new List<ArticleAdminReadDto> { };
            var pagedList = PagedList<ArticleAdminReadDto>.Create(articles.AsQueryable(), 1, 10);

            _serviceMock.Setup(s => s.GetAllArticlesNotCheckedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(pagedList);

            //Act
            var result = await _controller.GetAllArticlesNotCheckedAsync(null, 1, 10);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<PagedList<ArticleAdminReadDto>>(okResult.Value);
            Assert.Equal(0, returnedList.TotalCount);
        }

        [Fact]
        public async Task GetAllArticlesNotCheckedAsync_ReturnsArticles()
        {
            //Arrange
            var articles = new List<ArticleAdminReadDto> {
                new ArticleAdminReadDto {
                    Id = 6,
                    Title = "Title",
                    Author = "Author",
                    Content = "Content",
                    FeaturedImageURL = "FeaturedImageURL",
                    PublishedAt = new DateTime(2025, 1, 1),
                    IsRelevant = true
                },
                new ArticleAdminReadDto {
                    Id = 8,
                    Title = "Title1",
                    Author = "Author1",
                    Content = "Content1",
                    FeaturedImageURL = "FeaturedImageURL1",
                    PublishedAt = new DateTime(2025, 2, 1),
                    IsRelevant = false
                }
            };

            var pagedList = PagedList<ArticleAdminReadDto>.Create(articles.AsQueryable(), 1, 10);

            _serviceMock.Setup(s => s.GetAllArticlesNotCheckedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(pagedList);

            //Act
            var result = await _controller.GetAllArticlesNotCheckedAsync(null, 1, 10);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<PagedList<ArticleAdminReadDto>>(okResult.Value);

            Assert.Equal(2, returnedList.TotalCount);
            Assert.Equal("Title", returnedList.Items[0].Title);
            Assert.Equal("Title1", returnedList.Items[1].Title);
            Assert.Equal("Content", returnedList.Items[0].Content);
            Assert.Equal("Content1", returnedList.Items[1].Content);
        }

        //GetArticleByIdAsync

        [Fact]
        public async Task GetArticleByIdAsync_ReturnsNotFound()
        {
            //Arrange
            _serviceMock.Setup(s => s.GetArticleByIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((ArticleAdminReadDto?)null);

            //Act
            var result = await _controller.GetArticleByIdAsync(7);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetArticleByIdAsync_ReturnsOk()
        {
            //Arrange
            var article = new ArticleAdminReadDto
            {
                Id = 7,
                Title = "Title",
                Author = "Author",
                Content = "Content",
                FeaturedImageURL = "FeaturedImageURL",
                PublishedAt = new DateTime(2025, 3, 1),
                IsRelevant = false
            };

            _serviceMock.Setup(s => s.GetArticleByIdAsync(7))
                        .ReturnsAsync(article);

            //Act
            var result = await _controller.GetArticleByIdAsync(7);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<ArticleAdminReadDto>(okResult.Value);
            Assert.Equal(7, returned.Id);
            Assert.Equal("Title", returned.Title);
        }

        ////UpdateAsync 

        [Fact]
        public async Task UpdateAsync_ReturnsNotFound()
        {
            //Arrange
            var request = new ArticleAdminUpdateRequest
            {
                IsChecked = true,
                ClusterId = 5,
                DetachCluster = false
            };

            _serviceMock.Setup(s => s.UpdateAsync(7, It.IsAny<ArticleAdminUpdateRequest>()))
                        .ReturnsAsync((ArticleAdminUpdateDto?)null);

            //Act
            var result = await _controller.UpdateAsync(7, request);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsOk()
        {
            //Arrange
            var dto = new ArticleAdminUpdateDto
            {
                IsChecked = false,
                ClusterId = 10
            };

            var request = new ArticleAdminUpdateRequest
            {
                IsChecked = false,
                ClusterId = 10,
                DetachCluster = false
            };

            _serviceMock.Setup(s => s.UpdateAsync(7, request))
                        .ReturnsAsync(dto);

            //Act
            var result = await _controller.UpdateAsync(7, request);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<ArticleAdminUpdateDto>(okResult.Value);

            Assert.False(returned.IsChecked);
            Assert.Equal(10, returned.ClusterId);
            _serviceMock.Verify(s => s.UpdateAsync(7, request), Times.Once);
        }
    }
}
