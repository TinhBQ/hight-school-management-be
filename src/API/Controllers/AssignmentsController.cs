using DomainModel.Responses;
using Entities.DTOs.CRUD;
using Entities.RequestFeatures;
using Entities.Responses;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.IApplicationServices;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/assignments")]
    [ApiController]
    public class AssignmentsController(IAssignmentService service) : ControllerBase
    {
        private readonly IAssignmentService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAssignments([FromQuery] AssignmentParameters assignmentParameters)
        {
            var (assignments, metaData) = await _service.GetAssignmentsAsync(assignmentParameters);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            var response = new ResponseBase<IEnumerable<AssignmentDTO>>
            {
                Data = assignments,
                Message = ResponseMessage.GetClassesSuccess
            };

            return Ok(response);
        }
    }
}
