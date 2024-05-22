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
    [Route("api/subjects-for-class")]
    [ApiController]
    public class SubjectsForClassController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpGet]
        public async Task<IActionResult> GetSubjectClassByClassId([FromQuery] SubjectsForClassParameters subjectsForClassParameters)
        {
            var (subjectClasses, metaData) = await _service.SubjectClassService.GetSubjectClasByClassId(subjectsForClassParameters, trackChanges: false);

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            var response = new ResponseBase<IEnumerable<SubjectClassDTO>>
            {
                Data = subjectClasses,
                Message = ResponseMessage.GetClassesSuccess,
            };

            return Ok(response);
        }
    }
}
