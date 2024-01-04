using Application.Services.UserService;
using Domain.DomainObjects.Users;
using Microsoft.AspNetCore.Mvc;

namespace Updater.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;
        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        public class UserDTO
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] UserDTO userDTO)
        {
            IEnumerable<User> searchResult = await _userService.SearchAsync(new UserSearchPredicate() { Username = userDTO.Username });

            if (searchResult.Any())
            {
                throw new NotSupportedException();
            }

            User user = new User()
            {
                Username = userDTO.Username,
                Password = userDTO.Password,
            };

            await _userService.SaveAsync(user);

            return Ok();
        }
    }
}
