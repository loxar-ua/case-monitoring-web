using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using shkandal_api.Controllers;
using shkandalData.DTOs.UserDtos;
using ShkandalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkanadalTests.ControllersTests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _serviceMock;
        private readonly AuthController _controller;
        public AuthControllerTests()
        {
            _serviceMock = new Mock<IAuthService>();
            _controller = new AuthController(_serviceMock.Object);
        }

        //Login

        [Fact]
        public async Task Login_Success_ReturnsOkWithToken()
        {
            // Arrange
            var request = new AdminLoginRequest
            {
                Username = "name",
                Password = "Password"
            };

            var fakeToken = "fake-jwt-token";

            _serviceMock
                .Setup(s => s.LoginAdmin(request.Username, request.Password))
                .ReturnsAsync(fakeToken);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var tokenProperty = okResult.Value.GetType().GetProperty("token");
            var tokenValue = (string)tokenProperty.GetValue(okResult.Value);

            Assert.Equal(fakeToken, tokenValue);

            _serviceMock.Verify(s => s.LoginAdmin(request.Username, request.Password), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new AdminLoginRequest
            {
                Username = "name",
                Password = "wrongPass"
            };

            _serviceMock
                .Setup(s => s.LoginAdmin(request.Username, request.Password))
                .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials", unauthorized.Value);
        }

    }
}
