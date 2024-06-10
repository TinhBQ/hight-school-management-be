using API.ModelBinders;
using DomainModel.Responses;
using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.RequestFeatures;
using Entities.Responses;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.IApplicationServices;
using System.Text.Json;

namespace API.Presentation.Controllers
{
    [Route("api/class-with-homeroom-teachers")]
    [ApiController]
    public class HomeroomAssignmentsController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpGet]
        public async Task<IActionResult> GetClasses([FromQuery] ClassParameters classParameters)
        {
            var (classForHomeroomAssignments, metaData) = await _service.ClassService.GetAllHomeroomAssignment(classParameters, trackChanges: false, isInclude: true);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            var response = new ResponseBase<IEnumerable<ClassToHomeroomAssignmentDTO>>
            {
                Data = classForHomeroomAssignments,
                Message = ResponseMessage.GetClassesSuccess,
            };

            return Ok(response);
        }

        [HttpPut("{classId:guid}")]
        public async Task<IActionResult> UpdateClass(Guid classId, [FromBody] ClassToHomeroomAssignmentForUpdateDTO classToHomeroomAssignmentUpdate)
        {
            if (classToHomeroomAssignmentUpdate is null)
                return BadRequest("CompanyForUpdateDto object is null");

            await _service.ClassService.UpdateClassToHomeroomAssignmentAsync(classId, classToHomeroomAssignmentUpdate, trackChanges: true);

            return NoContent();
        }

        [HttpPut("collection")]
        public async Task<IActionResult> UpdateClassCollection([FromBody] IEnumerable<ClassToHomeroomAssignmentForUpdateCollectionDTO> ClassToHomeroomAssignmentForUpdateCollection)
        {
            if (ClassToHomeroomAssignmentForUpdateCollection is null)
                return BadRequest("CompanyForUpdateDto object is null");

            await _service.ClassService.UpdateClassToHomeroomAssignmentCollectionAsync(ClassToHomeroomAssignmentForUpdateCollection, trackChanges: true);

            return NoContent();
        }
    }
}
