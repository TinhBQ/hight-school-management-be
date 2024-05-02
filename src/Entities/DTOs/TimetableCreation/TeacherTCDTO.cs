﻿using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public record TeacherTCDTO
    {
        public Guid Id { get; init; }
        public string ShortName { get; init; }
        public int PeriodCount { get; init; }
        public Guid? ClassId { get; init; }

        public TeacherTCDTO(Teacher teacher)
        {
            Id = teacher.Id;
            ShortName = teacher.ShortName;
            PeriodCount = teacher.PeriodCount;
            ClassId = teacher.ClassId;
        }
    }
}
