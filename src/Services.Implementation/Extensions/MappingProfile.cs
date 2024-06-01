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

            CreateMap<Class, ClassDTO>();
            CreateMap<Class, ClassYearDTO>();
            CreateMap<ClassForCreationDTO, Class>();
            CreateMap<ClassForUpdateDTO, Class>();

            CreateMap<Class, ClassToHomeroomAssignmentDTO>();
            CreateMap<ClassToHomeroomAssignmentForUpdateDTO, Class>();

            CreateMap<Subject, SubjectDTO>();
            CreateMap<SubjectForCreationDTO, Subject>();
            CreateMap<SubjectForUpdateDTO, Subject>();

            CreateMap<Teacher, TeacherDTO>()
                .ForMember(c => c.FullName,
                                opt => opt.MapFrom(x => string.Join(' ', x.FirstName, x.MiddleName, x.LastName)));
            CreateMap<TeacherForCreationDTO, Teacher>();
            CreateMap<TeacherForUpdateDTO, Teacher>();

            CreateMap<SubjectClass, SubjectClassDTO>();
            CreateMap<SubjectClassForCreationDTO, SubjectClass>();
            CreateMap<SubjectClassForUpdateDTO, SubjectClass>();

            CreateMap<TimetableUnitTCDTO, TimetableUnit>();
        }
    }
}
