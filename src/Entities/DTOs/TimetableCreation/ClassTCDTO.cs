﻿using Entities.Common;
using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public record ClassTCDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int Grade { get; init; }
        public ESchoolShift? SchoolShift { get; init; }
        public int? PeriodCount { get; init; }
        public Guid? HomeroomTeacherId { get; init; }

        public ClassTCDTO(Class @class)
        {
            Id = @class.Id;
            Name = @class.Name;
            Grade = @class.Grade;
            SchoolShift = @class.SchoolShift;
            PeriodCount = @class.PeriodCount;
            HomeroomTeacherId = @class.HomeroomTeacherId;
        }
    }
}
