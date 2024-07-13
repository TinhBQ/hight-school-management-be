using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.IApplicationServices;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService service) : ControllerBase
    {
        private readonly IAuthenticationService _service = service;

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _service.Login(username, password);
            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string token)
        {
            return Ok();
        }

        [HttpPost("genaccounts")]
        public async Task<IActionResult> GenerateAccount()
        {
            await _service.GenerateAccount([]);
            return Ok();
        }
    }
}
