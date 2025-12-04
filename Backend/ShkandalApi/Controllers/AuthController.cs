using Microsoft.AspNetCore.Mvc;
using shkandalData.DTOs.UserDtos;
using ShkandalServices;

namespace shkandal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginRequest request)
        {
            try {
                var token = await _auth.LoginAdmin(request.Username, request.Password);

                return Ok(new { token });
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
