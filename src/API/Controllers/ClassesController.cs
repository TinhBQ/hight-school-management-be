using API.ModelBinders;
using Entities.DTOs;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.IApplicationServices;
using System.Text.Json;

namespace API.Presentation.Controllers
{
    [Route("api/classes")]
    [ApiController]
    public class ClassesController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpGet]
        public async Task<IActionResult> GetClasses([FromQuery] ClassParameters classParameters)
        {
            var (classes, metaData) = await _service.ClassService.GetAllClassesAsync(classParameters, trackChanges: false);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            return Ok(classes);
        }

        [HttpGet("{id:guid}", Name = "ClassById")]
        public async Task<IActionResult> GetClass(Guid id)
        {
            var klass = await _service.ClassService.GetClassAsync(id, trackChanges: false);
            return Ok(klass);
        }

        [HttpGet("collection/({ids})", Name = "ClassCollection")]
        public async Task<IActionResult> GetClassCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var classes = await _service.ClassService.GetByIdsAsync(ids, trackChanges: false);
            return Ok(classes);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateClassCollection([FromBody] IEnumerable<ClassForCreationDTO> classCollection)
        {
            var (classes, ids) = await _service.ClassService.CreateClassCollectionAsync(classCollection);

            return CreatedAtRoute("ClassCollection", new { ids }, classes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] ClassForCreationDTO klass)
        {
            if (klass is null)
                return BadRequest("CompanyForCreationDto object is null");

            var createdClass = await _service.ClassService.CreateClassAsync(klass);

            return CreatedAtRoute("ClassById", new { id = createdClass.Id }, createdClass);
        }

        [HttpPut("{id:guid}")]
        /*[ServiceFilter(typeof(ValidationFilterAttribute))]*/
        public async Task<IActionResult> UpdateClass(Guid id, [FromBody] ClassForUpdateDTO klass)
        {
            if (klass is null)
                return BadRequest("CompanyForUpdateDto object is null");

            await _service.ClassService.UpdateClassAsync(id, klass, trackChanges: true);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteClass(Guid id)
        {
            await _service.ClassService.DeleteClassAsync(id, trackChanges: false);
            return NoContent();
        }

        [HttpDelete("collection/({ids})")]

        public async Task<IActionResult> DeleteClassCollection(IEnumerable<Guid> ids)
        {
            await _service.ClassService.DeleteClassCollectionAsync(ids, trackChanges: false);
            return NoContent();
        }
    }
}
