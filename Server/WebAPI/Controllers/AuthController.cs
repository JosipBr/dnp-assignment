using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;  // Assuming you have IUserRepository for accessing user data
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public AuthController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = userRepository.GetMany().FirstOrDefault(u => u.Username == loginRequest.UserName);

            if (user == null || user.Password != loginRequest.Password)
            {
                return Unauthorized("Invalid credentials");
            }
            
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.Username,
            };
            
            return Ok(userDto);
        }
    }
}