﻿using AutoMapper;
using Entities.DAOs;
using Entities.DTOs;


namespace Services.Implementation.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Class, ClassDTO>();
            CreateMap<ClassForCreationDTO, Class>();
            CreateMap<ClassForUpdateDTO, Class>();

            CreateMap<Subject, SubjectDTO>();
            CreateMap<SubjectForCreationDTO, Subject>();
            CreateMap<SubjectForUpdateDTO, Subject>();

            CreateMap<Teacher, TeacherDTO>();
            CreateMap<TeacherForCreationDTO, Teacher>();
            CreateMap<TeacherForUpdateDTO, Teacher>();
        }
    }
}