using AutoMapper;
using Contexts;
using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.Exceptions;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction.IApplicationServices;
using Services.Abstraction.ILoggerServices;
using Services.Abstraction.IRepositoryServices;

namespace Services.Implementation
{
    internal sealed class AssignmentBQTService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IHelperService helperService) : IAssignmentBQTService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IHelperService _helperService = helperService;

        public async Task<IEnumerable<AssignmentDTO>> GetAllAssignmentsAsync(AssignmentParameters parameters, bool trackChanges)
        {
            var assignments = await _repository.AssignmentRepository.GetAllAssignment(parameters, trackChanges);

            var assignmentsDTO = _mapper.Map<IEnumerable<AssignmentDTO>>(assignments);

            return assignmentsDTO;
        }

        public async Task<(IEnumerable<AssignmentDTO>, MetaData)> GetAssignmentsAsync(AssignmentParameters parameters, bool trackChanges)
        {
            var assignmentsWithMetaData = await _repository.AssignmentRepository.GetAllAssignmentWithPagedList(parameters, trackChanges);

            var assignmentsDTO = _mapper.Map<IEnumerable<AssignmentDTO>>(assignmentsWithMetaData);

            return (assignmentsDTO, assignmentsWithMetaData.metaData);

        }

        public async Task<IEnumerable<ClassDTO>> GetAssignmentWithClasses(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await _repository.AssignmentRepository.GetAllAssignment(assignmentParameters, trackChanges);

            var distinctClass = assignments
                                 .GroupBy(a => a.ClassId)
                                 .Select(g => g.First().Class)
                                 .ToList();

            var classesDTO = _mapper.Map<IEnumerable<ClassDTO>>(distinctClass);

            return classesDTO;
        }

        public async Task<IEnumerable<SubjectDTO>> GetAssignmentWithSubjects(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await _repository.AssignmentRepository.GetAllAssignment(assignmentParameters, trackChanges);

            var distinctSubject = assignments
                                .GroupBy(a => a.SubjectId)
                                .Select(g => g.First().Subject)
                                .ToList();


            var classesDTO = _mapper.Map<IEnumerable<SubjectDTO>>(distinctSubject);

            return classesDTO;
        }

        public async Task<IEnumerable<AssignmentSubjectDTO>> GetAssignmentWithSubjectsNotSameTeacher(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await _repository.AssignmentRepository.GetAllAssignment(assignmentParameters, trackChanges);

            List<Subject> resultSubjects = new List<Subject>();

            List<AssignmentSubjectDTO> assignmentSubjectDTOs = new List<AssignmentSubjectDTO>();

            var subjects = assignments
                .GroupBy(a => a.SubjectId)
                .Select(g => g.First().Subject)
                .ToList();

            var klasses = assignments
                .GroupBy(a => a.ClassId)
                .Select(g => g.First().Class)
                .ToList();

            foreach (var item in subjects)
            {
                var assignmentsBySubjectId = await _repository.AssignmentRepository.GetAllAssignmentBySubjectId(item.Id, assignmentParameters, trackChanges);

                var teachers = assignmentsBySubjectId.Select(a => a.Teacher);
                    
                if (teachers.Count() == teachers.Select(a => a.Id).Distinct().Count())
                {
                    var subjectDto = _mapper.Map<SubjectDTO>(item);

                    var classes = assignmentsBySubjectId
                       .GroupBy(a => a.ClassId)
                       .Select(g => g.First().Class)
                       .ToList();

                    List<AssignmentClassTeacherDTO> classTeacherDTOs = new List<AssignmentClassTeacherDTO>();
                    foreach (var klass in classes)
                    {
                        var classTeacherDTO = new AssignmentClassTeacherDTO();
                        var teacher = assignments.Where(c => c.SubjectId == subjectDto.Id && c.ClassId == klass.Id)
                            .Select(g => g.Teacher)
                            .FirstOrDefault();
                        var classesDTO = _mapper.Map<ClassDTO>(klass);
                        var teacherDTO = _mapper.Map<TeacherDTO>(teacher);

                        classTeacherDTO.Class = classesDTO;
                        classTeacherDTO.Teacher = teacherDTO;


                        classTeacherDTOs.Add(classTeacherDTO);
                    }

                    var classesDTOs = _mapper.Map<IEnumerable<ClassDTO>>(classes);

                    var assignmentSubjectDTO = new AssignmentSubjectDTO();
                    assignmentSubjectDTO.Subject = subjectDto;
                    assignmentSubjectDTO.ClassTeachers = classTeacherDTOs;
                    assignmentSubjectDTO.PeriodCount = assignmentsBySubjectId.Min(a => a.PeriodCount);

                    assignmentSubjectDTOs.Add(assignmentSubjectDTO);
                }
            }

            return assignmentSubjectDTOs;
        }

        public async Task<IEnumerable<TeacherDTO>> GetAssignmentWithTeahers(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await _repository.AssignmentRepository.GetAllAssignment(assignmentParameters, trackChanges);

            var distinctTeachers = assignments
                                    .GroupBy(a => a.TeacherId)
                                    .Select(g => g.First().Teacher)
                                    .ToList();

            var classesDTO = _mapper.Map<IEnumerable<TeacherDTO>>(distinctTeachers);

            return classesDTO;
        }
    }
}

