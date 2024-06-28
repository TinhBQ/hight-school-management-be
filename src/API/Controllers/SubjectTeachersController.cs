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
    [Route("api/subjects-teachers")]
    [ApiController]
    public class SubjectTeachersController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllSubjectTeacher([FromQuery] SubjectTeacherParameters subjectTeacherParameters)
        {
            var (subjectTeachers, metaData) = await _service.SubjectTeacherService.GetAllSubjectTeacher(subjectTeacherParameters, trackChanges: false);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            var response = new ResponseBase<IEnumerable<SubjectTeacherDTO>>
            {
                Data = subjectTeachers,
                Message = ResponseMessage.GetClassesSuccess,
            };

            return Ok(response);
        }

        [HttpGet("{id:guid}", Name = "SubjectTeacherById")]
        public async Task<IActionResult> GetSubjectTeacherById(Guid id)
        {
            var subjectTeacher = await _service.SubjectTeacherService.GetSubjectTeacher(id, trackChanges: false);

            var response = new ResponseBase<SubjectTeacherDTO>
            {
                Data = subjectTeacher,
                Message = ResponseMessage.GetClassSuccess,
            };

            return Ok(response);
        }

        [HttpGet("collection/({ids})", Name = "SubjectTeacherCollection")]
        public async Task<IActionResult> GetSubjectTeacherCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var subjectTeachers = await _service.SubjectTeacherService.GetByIds(ids, trackChanges: false);

            var response = new ResponseBase<IEnumerable<SubjectTeacherDTO?>?>
            {
                Data = subjectTeachers,
                Message = ResponseMessage.GetClassSuccess,
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubjectTeacher([FromBody] SubjectTeacherForCreationDTO subjectTeacher)
        {
            if (subjectTeacher is null)
                return BadRequest("CompanyForCreationDto object is null");

            var _subjectTeacher = await _service.SubjectTeacherService.CreateSubjectTeacher(subjectTeacher);

            return CreatedAtRoute("SubjectTeacherById", new { id = _subjectTeacher.Id }, _subjectTeacher);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateSubjectTeacherCollection([FromBody] IEnumerable<SubjectTeacherForCreationDTO> subjectTeachers)
        {
            var (_subjectTeachers, ids) = await _service.SubjectTeacherService.CreateSubjcetTeacherCollection(subjectTeachers);

            return CreatedAtRoute("SubjectTeacherCollection", new { ids }, _subjectTeachers);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSubjectTeacher(Guid id, [FromBody] SubjectTeacherForUpdateDTO subjectTeacher)
        {
            if (subjectTeacher is null)
                return BadRequest("CompanyForUpdateDto object is null");

            await _service.SubjectTeacherService.UpdateSubjectTeacher(id, subjectTeacher, trackChanges: true);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSubjectTeacher(Guid id)
        {
            await _service.SubjectTeacherService.DeleteSubjectTeacher(id, trackChanges: true);

            return NoContent();
        }

        [HttpDelete("collection/({ids})")]

        public async Task<IActionResult> DeleteSubjectTeacherCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            await _service.SubjectTeacherService.DeleteSubjectTeacherCollection(ids, trackChanges: true);
            return NoContent();
        }
    }
}
