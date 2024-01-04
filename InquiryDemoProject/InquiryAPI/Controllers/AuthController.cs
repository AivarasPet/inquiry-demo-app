using Application.Services.UserService;
using Application.Utils;
using Domain.DomainObjects.Users;
using InquiryAPI.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace InquiryAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _userService;
        private readonly string _jwtSecretKey;
        public AuthController(AppConfiguration appConfiguration, IUsersService userService)
        {
            _jwtSecretKey = appConfiguration.JwtSecretKey;
            _userService = userService;
        }

        public class LoginDTO
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            (bool, User) isUserValid = await _userService.ValidateCredentialsAsync(loginDTO.Username, loginDTO.Password);
            if (isUserValid.Item1)
            {
                var token = JwtUtils.GenerateJwtToken(isUserValid.Item2.Id, _jwtSecretKey);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}
