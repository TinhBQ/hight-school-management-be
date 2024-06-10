using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.IApplicationServices;

namespace API.Controllers
{
    [Route("api/database")]
    [ApiController]
    public class DatabaseController(
        IDatabaseService databaseService)
        : Controller
    {
        private readonly IDatabaseService _databaseService = databaseService;

        [HttpGet("data-initialization")]
        public IActionResult InitData()
        {
            _databaseService.InitData();
            return Ok();
        }
    }
}
