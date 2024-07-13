using AutoMapper;
using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.DTOs.TimetableCreation;


namespace Services.Implementation.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Assignment, AssignmentDTO>()
                .DisableCtorValidation()
                .ForMember(des => des.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.FirstName} {src.Teacher.MiddleName} {src.Teacher.LastName}"))
                .ForMember(des => des.TeacherShortName, opt => opt.MapFrom(src => src.Teacher.ShortName))
                .ForMember(des => des.ClassName, opt => opt.MapFrom(src => src.Class.Name))
                .ForMember(des => des.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));
            CreateMap<AssignmentDTO, Assignment>();
            CreateMap<AssignmentForCreationDTO, Assignment>();
            CreateMap<AssignmentForUpdateDTO, Assignment>();

            CreateMap<Class, ClassDTO>();
            CreateMap<Class, ClassYearDTO>();
            CreateMap<ClassForCreationDTO, Class>();
            CreateMap<ClassForUpdateDTO, Class>();
            CreateMap<ClassTCDTO, ClassDTO>();

            CreateMap<Class, ClassToHomeroomAssignmentDTO>();
            CreateMap<ClassToHomeroomAssignmentForUpdateDTO, Class>();
            CreateMap<ClassToHomeroomAssignmentForUpdateCollectionDTO, Class>();

            CreateMap<Subject, SubjectDTO>();
            CreateMap<Subject, SubjectTCDTO>();
            CreateMap<SubjectForCreationDTO, Subject>();
            CreateMap<SubjectForUpdateDTO, Subject>();
            CreateMap<SubjectTCDTO, SubjectDTO>();

            CreateMap<Teacher, TeacherDTO>()
                .DisableCtorValidation()
                .ForMember(c => c.FullName,
                                opt => opt.MapFrom(x => string.Join(' ', x.FirstName, x.MiddleName, x.LastName)));
            CreateMap<TeacherForCreationDTO, Teacher>();
            CreateMap<TeacherForUpdateDTO, Teacher>();
            CreateMap<TeacherTCDTO, TeacherTCDTO>();

            CreateMap<SubjectClass, SubjectClassDTO>();
            CreateMap<SubjectClassForCreationDTO, SubjectClass>();
            CreateMap<SubjectClassForUpdateDTO, SubjectClass>();

            CreateMap<SubjectTeacher, SubjectTeacherDTO>();
            CreateMap<SubjectTeacherForCreationDTO, SubjectTeacher>();
            CreateMap<SubjectTeacherForUpdateDTO, SubjectTeacher>();

            CreateMap<TimetableUnitTCDTO, TimetableUnit>();
            CreateMap<TimetableUnit, TimetableUnitTCDTO>();

            CreateMap<TimetableIndividual, TimetableDTO>();
            CreateMap<TimetableIndividual, Timetable>();
            CreateMap<TimetableDTO, TimetableIndividual>();
            CreateMap<TimetableDTO, Timetable>();
            CreateMap<Timetable, TimetableDTO>();
            CreateMap<Timetable, TimetableIndividual>();
        }
    }
}
