using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.IApplicationServices;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService service) : ControllerBase
    {
        private readonly IAuthenticationService _service = service;

        [HttpPost]
        public async Task<IActionResult> Login()
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
