using AutoMapper;
using Entities.DAOs;
using Entities.DTOs.CRUD;


namespace Services.Implementation.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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

            CreateMap<SubjectTeacher, SubjectTeacherDTO>();
        }
    }
}
