using Entities.Common;
using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasData
                (
                new Class
                {
                    Id = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"),
                    Grade = 10,
                    Name = "10B1",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"),
                    Grade = 10,
                    Name = "10B2",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"),
                    Grade = 10,
                    Name = "10B3",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"),
                    Grade = 10,
                    Name = "10B4",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"),
                    Grade = 10,
                    Name = "10B5",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"),
                    Grade = 10,
                    Name = "10B6",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("a159920c-504f-498a-b225-79844ad120ae"),
                    Grade = 10,
                    Name = "10B7",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"),
                    Grade = 10,
                    Name = "10B8",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"),
                    Grade = 11,
                    Name = "11A1",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Afternoon
                },
                new Class
                {
                    Id = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"),
                    Grade = 11,
                    Name = "11A2",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Afternoon
                },
                new Class
                {
                    Id = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"),
                    Grade = 11,
                    Name = "11A3",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Afternoon
                },
                new Class
                {
                    Id = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"),
                    Grade = 11,
                    Name = "11A4",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Afternoon
                },
                new Class
                {
                    Id = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"),
                    Grade = 11,
                    Name = "11A5",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Afternoon
                },
                new Class
                {
                    Id = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"),
                    Grade = 11,
                    Name = "11A6",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Afternoon
                },
                new Class
                {
                    Id = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"),
                    Grade = 11,
                    Name = "11A7",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Afternoon
                },
                new Class
                {
                    Id = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"),
                    Grade = 11,
                    Name = "11A8",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Afternoon
                },
                new Class
                {
                    Id = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"),
                    Grade = 11,
                    Name = "11A9",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Afternoon
                },
                new Class
                {
                    Id = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"),
                    Grade = 12,
                    Name = "12C1",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"),
                    Grade = 12,
                    Name = "12C2",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"),
                    Grade = 12,
                    Name = "12C3",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"),
                    Grade = 12,
                    Name = "12C4",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"),
                    Grade = 12,
                    Name = "12C5",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"),
                    Grade = 12,
                    Name = "12C6",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"),
                    Grade = 12,
                    Name = "12C7",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                },
                new Class
                {
                    Id = new Guid("f6123108-6be8-496d-9c60-7753f313368f"),
                    Grade = 12,
                    Name = "12C8",
                    StartYear = 2023,
                    EndYear = 2024,
                    PeriodCount = 25,
                    SchoolShift = ESchoolShift.Morning
                }
                );
        }
    }
}
