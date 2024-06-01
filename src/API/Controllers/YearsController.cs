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
    [Route("api/years")]
    [ApiController]
    public class YearsController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpGet()]
        public async Task<IActionResult> GetClassesYears([FromQuery] ClassParameters classParameters)
        {
            var (classes, metaData) = await _service.ClassService.GetYearsAsync(classParameters, trackChanges: false, isInclude: false);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            var response = new ResponseBase<IEnumerable<ClassYearDTO>>
            {
                Data = classes,
                Message = ResponseMessage.GetClassesSuccess,
            };

            return Ok(response);
        }
    }
}
