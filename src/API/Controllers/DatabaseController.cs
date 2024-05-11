using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.IApplicationServices;

namespace API.Controllers
{
    [Route("api/database")]
    [ApiController]
    public class DatabaseController(
        IAssignmentServiceTemp assignmentService)
        : Controller
    {
        private readonly IAssignmentServiceTemp _assignmentService = assignmentService;

        [HttpGet("assignments/creation")]
        public IActionResult CreateAssignment()
        {
            _assignmentService.Create();
            return Ok();
        }

        [HttpDelete("assignments/deletion")]
        public IActionResult DeteleAssignment()
        {
            _assignmentService.Delete();
            return Ok();
        }
    }
}
