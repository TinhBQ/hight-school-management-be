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
            CreateMap<Class, ClassDTO>();
            CreateMap<Class, ClassYearDTO>();
            CreateMap<ClassForCreationDTO, Class>();
            CreateMap<ClassForUpdateDTO, Class>();

            CreateMap<Subject, SubjectDTO>();
            CreateMap<SubjectForCreationDTO, Subject>();
            CreateMap<SubjectForUpdateDTO, Subject>();

            CreateMap<Teacher, TeacherDTO>();
            CreateMap<TeacherForCreationDTO, Teacher>();
            CreateMap<TeacherForUpdateDTO, Teacher>();

            CreateMap<SubjectClass, SubjectClassDTO>();
            CreateMap<SubjectClassForCreationDTO, SubjectClass>();
            CreateMap<SubjectClassForUpdateDTO, SubjectClass>();

            CreateMap<TimetableUnitTCDTO, TimetableUnit>();
        }
    }
}
