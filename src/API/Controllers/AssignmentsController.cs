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
    public class AssignmentsController(IAssignmentService service, IServiceManager serviceBQT) : ControllerBase
    {
        private readonly IAssignmentService _service = service;

        private readonly IServiceManager _serviceBQT = serviceBQT;

        [HttpGet]
        public async Task<IActionResult> GetAssignments([FromQuery] AssignmentParameters assignmentParameters)
        {
            var (assignments, metaData) = await _serviceBQT.AssignmentBQTService.GetAssignmentsAsync(assignmentParameters, trackChanges: false);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

            var response = new ResponseBase<IEnumerable<AssignmentDTO>>
            {
                Data = assignments,
                Message = ResponseMessage.GetClassesSuccess
            };

            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAssignments([FromQuery] AssignmentParameters assignmentParameters)
        {
            var assignments = await _serviceBQT.AssignmentBQTService.GetAllAssignmentsAsync(assignmentParameters, trackChanges: false);

            var response = new ResponseBase<IEnumerable<AssignmentDTO>>
            {
                Data = assignments,
                Message = ResponseMessage.GetClassesSuccess
            };

            return Ok(response);
        }


        [HttpGet("teacher")]
        public async Task<IActionResult> GetAssignmentsWithTeacher([FromQuery] AssignmentParameters assignmentParameters)
        {
            var  teachers= await _serviceBQT.AssignmentBQTService.GetAssignmentWithTeahers(assignmentParameters, false);

            var response = new ResponseBase<IEnumerable<TeacherDTO>>
            {
                Data = teachers,
                Message = ResponseMessage.GetClassesSuccess
            };

            return Ok(response);
        }

        [HttpGet("class")]
        public async Task<IActionResult> GetAssignmentsWithClass([FromQuery] AssignmentParameters assignmentParameters)
        {
            var klasses = await _serviceBQT.AssignmentBQTService.GetAssignmentWithClasses(assignmentParameters, false);

            var response = new ResponseBase<IEnumerable<ClassDTO>>
            {
                Data = klasses,
                Message = ResponseMessage.GetClassesSuccess
            };

            return Ok(response);
        }

        [HttpGet("subject")]
        public async Task<IActionResult> GetAssignmentsWithSubject([FromQuery] AssignmentParameters assignmentParameters)
        {
            var subjects = await _serviceBQT.AssignmentBQTService.GetAssignmentWithSubjects(assignmentParameters, false);

            var response = new ResponseBase<IEnumerable<SubjectDTO>>
            {
                Data = subjects,
                Message = ResponseMessage.GetClassesSuccess
            };

            return Ok(response);
        }

        [HttpGet("subject-not-same-teacher")]
        public async Task<IActionResult> GetAssignmentsWithSubjectNotSameTeacher([FromQuery] AssignmentParameters assignmentParameters)
        {
            var subjects = await _serviceBQT.AssignmentBQTService.GetAssignmentWithSubjectsNotSameTeacher(assignmentParameters, false);

            var response = new ResponseBase<IEnumerable<AssignmentSubjectDTO>>
            {
                Data = subjects,
                Message = ResponseMessage.GetClassesSuccess
            };

            return Ok(response);
        }
    }
}
