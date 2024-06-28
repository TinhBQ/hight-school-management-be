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
    [Route("api/teachers")]
    [ApiController]
    public class TeachersController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpGet]
        public async Task<IActionResult> GetTeachers([FromQuery] TeacherParameters teacherParameters)
        {
            var (teachers, metaData) = await _service.TeacherService.GetAllTeachersAsync(teacherParameters, trackChanges: false);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            var response = new ResponseBase<IEnumerable<TeacherDTO>>
            {
                Data = teachers,
                Message = ResponseMessage.GetClassesSuccess,
            };

            return Ok(response);
        }

        [HttpGet("{id:guid}", Name = "TeacherById")]
        public async Task<IActionResult> GetTeacher(Guid id)
        {
            var teacher = await _service.TeacherService.GetTeacherAsync(id, trackChanges: false);

            var response = new ResponseBase<TeacherDTO>
            {
                Data = teacher,
                Message = ResponseMessage.GetClassSuccess,
            };

            return Ok(response);
        }

        [HttpGet("collection/({ids})", Name = "TeacherCollection")]
        public async Task<IActionResult> GetTeacherCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var teachers = await _service.TeacherService.GetByIdsAsync(ids, trackChanges: false);

            var response = new ResponseBase<IEnumerable<TeacherDTO?>?>
            {
                Data = teachers,
                Message = ResponseMessage.GetClassSuccess,
            };

            return Ok(teachers);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateTeacherCollection([FromBody] IEnumerable<TeacherForCreationDTO> teacherCollection)
        {
            var (teachers, ids) = await _service.TeacherService.CreateTeacherCollectionAsync(teacherCollection);

            return CreatedAtRoute("TeacherCollection", new { ids }, teachers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherForCreationDTO teacher)
        {
            if (teacher is null)
                return BadRequest("CompanyForCreationDto object is null");

            var createdTeacher = await _service.TeacherService.CreateTeacherAsync(teacher);

            return CreatedAtRoute("TeacherById", new { id = createdTeacher.Id }, createdTeacher);
        }

        [HttpPut("{id:guid}")]
        /*[ServiceFilter(typeof(ValidationFilterAttribute))]*/
        public async Task<IActionResult> UpdateTeacher(Guid id, [FromBody] TeacherForUpdateDTO teacher)
        {
            if (teacher is null)
                return BadRequest("CompanyForUpdateDto object is null");

            await _service.TeacherService.UpdateTeacherAsync(id, teacher, trackChanges: true);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            await _service.TeacherService.DeleteTeacherAsync(id, trackChanges: true);
            return NoContent();
        }

        [HttpDelete("collection/({ids})")]

        public async Task<IActionResult> DeleteTeacherCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            await _service.TeacherService.DeleteTeacherCollectionAsync(ids, trackChanges: true);
            return NoContent();
        }
    }
}
