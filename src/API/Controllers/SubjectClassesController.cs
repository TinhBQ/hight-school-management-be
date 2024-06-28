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
    [Route("api/subjects-classes")]
    [ApiController]
    public class SubjectClassesController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpGet]
        public async Task<IActionResult> GetSubjectClasses([FromQuery] SubjectClassParameters subjectClassParameters)
        {
            var (subjectClasses, metaData) = await _service.SubjectClassService.GetAllSubjectClassesAsync(subjectClassParameters, trackChanges: false);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            var response = new ResponseBase<IEnumerable<SubjectClassDTO>>
            {
                Data = subjectClasses,
                Message = ResponseMessage.GetClassesSuccess,
            };

            return Ok(response);
        }

        [HttpGet("{id:guid}", Name = "SubjectClassById")]
        public async Task<IActionResult> GetSubjectClass(Guid id)
        {
            var subjectClassDTO = await _service.SubjectClassService.GetSubjectClassAsync(id, trackChanges: false);

            var response = new ResponseBase<SubjectClassDTO>
            {
                Data = subjectClassDTO,
                Message = ResponseMessage.GetClassSuccess,
            };

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> CreateSubjectClass([FromBody] SubjectClassForCreationDTO subjectClass)
        {
            if (subjectClass is null)
                return BadRequest("CompanyForCreationDto object is null");

            var createSubjectClas = await _service.SubjectClassService.CreateSubjectClassAsync(subjectClass);

            return CreatedAtRoute("SubjectClassById", new { id = createSubjectClas.Id }, createSubjectClas);
        }

        [HttpGet("collection/({ids})", Name = "SubjectClassCollection")]
        public async Task<IActionResult> GetSubjectClassCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var subjectsClasses = await _service.SubjectClassService.GetByIdsAsync(ids, trackChanges: false);

            var response = new ResponseBase<IEnumerable<SubjectClassDTO?>>
            {
                Data = subjectsClasses,
                Message = ResponseMessage.GetClassesSuccess,
            };

            return Ok(response);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateSubjectClassCollection([FromBody] IEnumerable<SubjectClassForCreationDTO> subjectClassCollection)
        {
            var (subjectsClasses, ids) = await _service.SubjectClassService.CreateSubjcetClassCollectionAsync(subjectClassCollection);

            return CreatedAtRoute("SubjectClassCollection", new { ids }, subjectClassCollection);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSubjectClass(Guid id, [FromBody] SubjectClassForUpdateDTO subjectClass)
        {
            if (subjectClass is null)
                return BadRequest("CompanyForUpdateDto object is null");

            await _service.SubjectClassService.UpdateSubjectClassAsync(id, subjectClass, trackChanges: true);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSubjectClass(Guid id)
        {
            await _service.SubjectClassService.DeleteSubjectClassAsync(id, trackChanges: true);
            return NoContent();
        }

        [HttpDelete("collection/({ids})")]

        public async Task<IActionResult> DeleteDeleteSubjectCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            await _service.SubjectClassService.DeleteSubjectClassCollectionAsync(ids, trackChanges: true);
            return NoContent();
        }
    }
}
