using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Grade).IsRequired();
            entity.Property(e => e.SchoolShift).HasConversion<int>();
            entity.Property(e => e.StartYear).IsRequired();
            entity.Property(e => e.EndYear).IsRequired();
            entity.Property(e => e.PeriodCount).IsRequired();

            entity.HasOne(e => e.HomeroomTeacher)
                .WithMany(e => e.Classes)
                .HasForeignKey(e => e.HomeroomTeacherId)
                .HasConstraintName("FK__Class__Teacher__ClassId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);


            //entity.HasData
            //    (
            //    new Class
            //    {
            //        Id = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"),
            //        Grade = 10,
            //        Name = "10B1",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("6749a629-6964-43f4-bc38-2173b963413c"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"),
            //        Grade = 10,
            //        Name = "10B2",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("f6a2d0e1-fcdc-4ea0-9c40-ffea09fd9490"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"),
            //        Grade = 10,
            //        Name = "10B3",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("a1a9f708-9a48-42fd-9308-3a106f9be31f"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"),
            //        Grade = 10,
            //        Name = "10B4",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("e7dc5f6e-0671-48ec-a298-9bd7ab8ffbf1"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"),
            //        Grade = 10,
            //        Name = "10B5",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("e936ed8e-b9ff-448c-b641-2cede0b5c9da"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"),
            //        Grade = 10,
            //        Name = "10B6",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("dae59f4a-9ee2-4995-8034-fd78c18d99bf"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("a159920c-504f-498a-b225-79844ad120ae"),
            //        Grade = 10,
            //        Name = "10B7",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("e57f2e3e-6335-4fb4-bb4a-c12a73b5d7cd"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"),
            //        Grade = 10,
            //        Name = "10B8",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("67174939-7dbf-44b7-a94d-42f3cc73457a"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"),
            //        Grade = 11,
            //        Name = "11A1",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Afternoon,
            //        HomeroomTeacherId = new Guid("647c4458-ab3e-45b5-9b71-7e109bfffdb5"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"),
            //        Grade = 11,
            //        Name = "11A2",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Afternoon,
            //        HomeroomTeacherId = new Guid("7f392915-ee09-4dcf-91ec-17d94ec39563"),

            //    },
            //    new Class
            //    {
            //        Id = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"),
            //        Grade = 11,
            //        Name = "11A3",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Afternoon,
            //        HomeroomTeacherId = new Guid("9f09c5a3-7799-40ed-911c-c5161c05cabc"),

            //    },
            //    new Class
            //    {
            //        Id = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"),
            //        Grade = 11,
            //        Name = "11A4",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Afternoon,
            //        HomeroomTeacherId = new Guid("fbcb8e24-1905-40fb-b49c-d27ff9ce20a2"),

            //    },
            //    new Class
            //    {
            //        Id = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"),
            //        Grade = 11,
            //        Name = "11A5",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Afternoon,
            //        HomeroomTeacherId = new Guid("5fe80a00-fa41-476c-a915-0238a70672cc"),

            //    },
            //    new Class
            //    {
            //        Id = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"),
            //        Grade = 11,
            //        Name = "11A6",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Afternoon,
            //        HomeroomTeacherId = new Guid("c1f9b122-383b-42bd-9a04-10964679b550"),

            //    },
            //    new Class
            //    {
            //        Id = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"),
            //        Grade = 11,
            //        Name = "11A7",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Afternoon,
            //        HomeroomTeacherId = new Guid("4986a2cc-8233-4a41-93ac-43e35e250c1a"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"),
            //        Grade = 11,
            //        Name = "11A8",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Afternoon,
            //        HomeroomTeacherId = new Guid("5e80e258-35bd-42b3-a6a5-cd46794565a9"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"),
            //        Grade = 11,
            //        Name = "11A9",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 25,
            //        SchoolShift = ESchoolShift.Afternoon,
            //        HomeroomTeacherId = new Guid("475b148d-8c8f-42a3-beae-f81c4f01ae3a"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"),
            //        Grade = 12,
            //        Name = "12C1",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 28,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("cb45fd82-caf7-49b3-8afa-07ad739cba97"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"),
            //        Grade = 12,
            //        Name = "12C2",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 28,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("df6cc373-ae5c-4cce-b000-c48af3fe3e2b"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"),
            //        Grade = 12,
            //        Name = "12C3",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 28,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("1a4e58d0-5e19-42c2-b73f-ac0d7b6988f1"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"),
            //        Grade = 12,
            //        Name = "12C4",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 27,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("8a557738-6e34-43c5-9ec9-3b45ac4fdea5"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"),
            //        Grade = 12,
            //        Name = "12C5",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 27,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("4f2b309b-1b10-4204-b627-7d376533ea15"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"),
            //        Grade = 12,
            //        Name = "12C6",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 27,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("0c6c9e47-e7c5-4d30-9945-65672be86dfc"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"),
            //        Grade = 12,
            //        Name = "12C7",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 27,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("b7adecee-f4d7-4a2d-8457-e5d27a427278"),
            //    },
            //    new Class
            //    {
            //        Id = new Guid("f6123108-6be8-496d-9c60-7753f313368f"),
            //        Grade = 12,
            //        Name = "12C8",
            //        StartYear = 2023,
            //        EndYear = 2024,
            //        PeriodCount = 27,
            //        SchoolShift = ESchoolShift.Morning,
            //        HomeroomTeacherId = new Guid("fb1a4dad-e5a9-416c-aad1-f0c98e394e49"),
            //    }
            //    );
        }
    }
}
