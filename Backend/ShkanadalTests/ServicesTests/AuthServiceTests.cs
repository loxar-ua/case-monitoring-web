using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using ShkandalData.Models;
using ShkandalInfrastructure.Repositories;
using ShkandalServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace ShkanadalTests.ServicesTests
{
    public class AuthServiceTests
    {
        private readonly AuthService _service;
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<IConfiguration> _configMock;

        public AuthServiceTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(c => c["Jwt:Key"]).Returns("SecretKey12345SecretKey12345SecretKey12345SecretKey12345");

            _service = new AuthService(_repositoryMock.Object, _configMock.Object);
        }


        //Login
        [Fact]
        public async Task LoginAdmin_UserNotFound_ThrowsUnauthorized()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetUserByUsernameAsync("Username"))
                .ReturnsAsync((User?)null);

            //Act and Assert
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _service.LoginAdmin("Username", "Password"));
            Assert.Equal("Admin not found", ex.Message);
        }

        [Fact]
        public async Task LoginAdmin_UserIsNotAdmind()
        {
            //Arrange
            var user = new User
            {
                Username = "Username",
                PasswordHash = "SomeHash",
                Role = User.UserRole.Visitor
            };

            _repositoryMock.Setup(r => r.GetUserByUsernameAsync("Username"))
                .ReturnsAsync(user);

            //Act and Assert
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _service.LoginAdmin("Username", "Password"));
            Assert.Equal("Admin not found", ex.Message);
        }

        [Fact]
        public async Task LoginAdmin_WrongPassword()
        {
            //Arrange
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Username = "AdminUser",
                Role = User.UserRole.Admin,
                PasswordHash = hasher.HashPassword(null!, "CorrectPassword")
            };

            _repositoryMock.Setup(r => r.GetUserByUsernameAsync("AdminUser"))
                .ReturnsAsync(user);


            //Act and Assert
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _service.LoginAdmin("AdminUser", "WrongPassword"));
            Assert.Equal("Invalid Password", ex.Message);
        }

        [Fact]
        public async Task LoginAdmin_ReturnsJwt()
        {
            //Arrange
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Username = "AdminUser",
                Role = User.UserRole.Admin,
                PasswordHash = hasher.HashPassword(null!, "PasswordHash")
            };

            _repositoryMock.Setup(r => r.GetUserByUsernameAsync("AdminUser"))
                .ReturnsAsync(user);

            //Act
            var tokenString = await _service.LoginAdmin("AdminUser", "PasswordHash");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

            //Assert
            Assert.False(string.IsNullOrEmpty(tokenString));
            Assert.Equal("AdminUser", token.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal(User.UserRole.Admin.ToString(), token.Claims.First(c => c.Type == ClaimTypes.Role).Value);
        }
    }
}
