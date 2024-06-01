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

        [HttpGet("assignments/creation")]
        public IActionResult CreateAssignment()
        {
            _databaseService.CreateAssignments();
            return Ok();
        }

        [HttpDelete("assignments/deletion")]
        public IActionResult DeteleAssignment()
        {
            _databaseService.CreateAssignments();
            return Ok();
        }

        [HttpPost("teachers")]
        public IActionResult UpdateHomeroomTeacher()
        {
            _databaseService.UpdateHomeroomTeacher();
            return Ok();
        }
    }
}
