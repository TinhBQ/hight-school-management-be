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
    [Route("api/subjects")]
    [ApiController]
    public class SubjectsController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpGet]
        public async Task<IActionResult> GetSubjects([FromQuery] SubjectParameters subjectParameters)
        {
            var (subjects, metaData) = await _service.SubjectService.GetAllSubjectsAsync(subjectParameters, trackChanges: false);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            var response = new ResponseBase<IEnumerable<SubjectDTO>>
            {
                Data = subjects,
                Message = ResponseMessage.GetSubjectsSuccess,
            };

            return Ok(response);
        }

        [HttpGet("unassigned-by-class")]
        public async Task<IActionResult> GetUnassignedSubjectsByClassId([FromQuery] Guid classId)
        {
            var subjects = await _service.SubjectService.GetUnassignedSubjectsByClassId(classId, trackChanges: false);

            var response = new ResponseBase<IEnumerable<SubjectDTO>>
            {
                Data = subjects,
                Message = ResponseMessage.GetSubjectsSuccess,
            };

            return Ok(response);
        }

        [HttpGet("assigned-by-class")]
        public async Task<IActionResult> GetAssignedSubjectsByClassId([FromQuery] Guid classId)
        {
            var subjects = await _service.SubjectService.GetAssignedSubjectsByClassId(classId, trackChanges: false);

            var response = new ResponseBase<IEnumerable<SubjectDTO>>
            {
                Data = subjects,
                Message = ResponseMessage.GetSubjectsSuccess,
            };

            return Ok(response);
        }

        [HttpGet("unassigned-by-teacher")]
        public async Task<IActionResult> GetUnassignedSubjectsByTeacherId([FromQuery] Guid teacherId)
        {
            var subjects = await _service.SubjectService.GetUnassignedSubjectsByTeacherId(teacherId, trackChanges: false);

            var response = new ResponseBase<IEnumerable<SubjectDTO>>
            {
                Data = subjects,
                Message = ResponseMessage.GetSubjectsSuccess,
            };

            return Ok(response);
        }

        [HttpGet("assigned-by-teacher")]
        public async Task<IActionResult> GetAssignedSubjectsByTeaherId([FromQuery] Guid teacherId)
        {
            var subjects = await _service.SubjectService.GetAssignedSubjectsByTeacherId(teacherId, trackChanges: false);

            var response = new ResponseBase<IEnumerable<SubjectDTO>>
            {
                Data = subjects,
                Message = ResponseMessage.GetSubjectsSuccess,
            };

            return Ok(response);
        }

        [HttpGet("{id:guid}", Name = "SubjectById")]
        public async Task<IActionResult> GetSubject(Guid id)
        {
            var subject = await _service.SubjectService.GetSubjectAsync(id, trackChanges: false);
            return Ok(subject);
        }

        [HttpGet("collection/({ids})", Name = "SubjectCollection")]
        public async Task<IActionResult> GetSubjectCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var subject = await _service.SubjectService.GetByIdsAsync(ids, trackChanges: false);
            return Ok(subject);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateSubjectCollection([FromBody] IEnumerable<SubjectForCreationDTO> subjectCollection)
        {
            var (subjects, ids) = await _service.SubjectService.CreateSubjectCollectionAsync(subjectCollection);

            return CreatedAtRoute("SubjectCollection", new { ids }, subjects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectForCreationDTO subject)
        {
            if (subject is null)
                return BadRequest("CompanyForCreationDto object is null");

            var createdSubject = await _service.SubjectService.CreateSubjectAsync(subject);

            return CreatedAtRoute("SubjectById", new { id = createdSubject.Id }, createdSubject);
        }

        [HttpPut("{id:guid}")]
        /*[ServiceFilter(typeof(ValidationFilterAttribute))]*/
        public async Task<IActionResult> UpdateSubject(Guid id, [FromBody] SubjectForUpdateDTO subject)
        {
            if (subject is null)
                return BadRequest("CompanyForUpdateDto object is null");

            await _service.SubjectService.UpdateSubjectAsync(id, subject, trackChanges: true);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            await _service.SubjectService.DeleteSubjectAsync(id, trackChanges: true);
            return NoContent();
        }

        [HttpDelete("collection/({ids})")]

        public async Task<IActionResult> DeleteSubjectCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            await _service.SubjectService.DeleteSubjectCollectionAsync(ids, trackChanges: true);
            return NoContent();
        }
    }
}
