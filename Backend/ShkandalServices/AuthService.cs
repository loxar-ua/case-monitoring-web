using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using shkandalData.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShkandalServices
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher = new();
        public AuthService(IUserRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public string HashPassword(User user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public async Task<string> LoginAdmin(string username, string password)
        {
            var user = await _repository.GetUserByUsername(username);
            if(user == null || user.Role != User.UserRole.Admin)
            {
                throw new UnauthorizedAccessException("Admin not found");
            }

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if(verify == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid Password");
            }

            return GenerateJwt(user);
        }

        private string GenerateJwt(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                },
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: new SigningCredentials(
                   new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

            return tokenHandler.WriteToken(token);
        }
        }
    }

