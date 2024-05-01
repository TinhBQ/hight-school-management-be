using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.PeriodCount).HasDefaultValue(0);
            entity.Property(e => e.SchoolShift).HasConversion<int>();
            entity.Property(e => e.Semester).IsRequired();
            entity.Property(e => e.StartYear).IsRequired();
            entity.Property(e => e.EndYear).IsRequired();

            entity.HasOne(e => e.Teacher)
                .WithMany(e => e.Assignments)
                .HasForeignKey(e => e.TeacherId)
                .HasConstraintName("FK__Assignment__Teacher__TeacherId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            entity.HasOne(e => e.Class)
                .WithMany(e => e.Assignments)
                .HasForeignKey(e => e.ClassId)
                .HasConstraintName("FK__Assignment__Class__ClassId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            entity.HasOne(e => e.Subject)
                .WithMany(e => e.Assignments)
                .HasForeignKey(e => e.SubjectId)
                .HasConstraintName("FK__Assignment__Subject__SubjectId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasData
               (
                    // Lê Thị Ngọc Ánh
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("b7adecee-f4d7-4a2d-8457-e5d27a427278"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("b7adecee-f4d7-4a2d-8457-e5d27a427278"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("b7adecee-f4d7-4a2d-8457-e5d27a427278"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("b7adecee-f4d7-4a2d-8457-e5d27a427278"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },

                    // Bùi Thị Ngọc Cẩm
                    // class 99dd07b4-0450-4efc-a70a-192d81577ced (10B2 9 môn)
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"), TeacherId = new Guid("67174939-7dbf-44b7-a94d-42f3cc73457a"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"), TeacherId = new Guid("67174939-7dbf-44b7-a94d-42f3cc73457a"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"), TeacherId = new Guid("67174939-7dbf-44b7-a94d-42f3cc73457a"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Đỗ Quốc Công
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"), TeacherId = new Guid("647c4458-ab3e-45b5-9b71-7e109bfffdb5"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"), TeacherId = new Guid("647c4458-ab3e-45b5-9b71-7e109bfffdb5"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"), TeacherId = new Guid("647c4458-ab3e-45b5-9b71-7e109bfffdb5"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"), TeacherId = new Guid("647c4458-ab3e-45b5-9b71-7e109bfffdb5"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },

                    // Bùi Thị Kiều Diễm
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"), TeacherId = new Guid("c750afe2-d099-4ead-ba1b-691261b75d32"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"), TeacherId = new Guid("c750afe2-d099-4ead-ba1b-691261b75d32"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("a159920c-504f-498a-b225-79844ad120ae"), TeacherId = new Guid("c750afe2-d099-4ead-ba1b-691261b75d32"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"), TeacherId = new Guid("c750afe2-d099-4ead-ba1b-691261b75d32"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"), TeacherId = new Guid("c750afe2-d099-4ead-ba1b-691261b75d32"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },

                    // Đỗ Đại Duy
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"), TeacherId = new Guid("dae59f4a-9ee2-4995-8034-fd78c18d99bf"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"), TeacherId = new Guid("dae59f4a-9ee2-4995-8034-fd78c18d99bf"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"), TeacherId = new Guid("dae59f4a-9ee2-4995-8034-fd78c18d99bf"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"), TeacherId = new Guid("dae59f4a-9ee2-4995-8034-fd78c18d99bf"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Phan Thị Đệ
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("a159920c-504f-498a-b225-79844ad120ae"), TeacherId = new Guid("e889a534-4a57-41f1-8d12-f81358bd2b82"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"), TeacherId = new Guid("e889a534-4a57-41f1-8d12-f81358bd2b82"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("e889a534-4a57-41f1-8d12-f81358bd2b82"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },

                    // Đỗ Thị Thu Hằng
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("9f09c5a3-7799-40ed-911c-c5161c05cabc"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"), TeacherId = new Guid("9f09c5a3-7799-40ed-911c-c5161c05cabc"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("9f09c5a3-7799-40ed-911c-c5161c05cabc"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"), TeacherId = new Guid("9f09c5a3-7799-40ed-911c-c5161c05cabc"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Nguyễn Thị Thanh Hoa (MT)
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("a159920c-504f-498a-b225-79844ad120ae"), TeacherId = new Guid("5e80e258-35bd-42b3-a6a5-cd46794565a9"), SubjectId = new Guid("c937c26a-de6b-4eb5-a9f3-58c3c37e17db") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"), TeacherId = new Guid("5e80e258-35bd-42b3-a6a5-cd46794565a9"), SubjectId = new Guid("c937c26a-de6b-4eb5-a9f3-58c3c37e17db") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"), TeacherId = new Guid("5e80e258-35bd-42b3-a6a5-cd46794565a9"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Nguyễn Thị Thanh Hoa (V)
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"), TeacherId = new Guid("5aaac979-05dd-4e9a-8174-6d4885a40ce7"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"), TeacherId = new Guid("5aaac979-05dd-4e9a-8174-6d4885a40ce7"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"), TeacherId = new Guid("5aaac979-05dd-4e9a-8174-6d4885a40ce7"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"), TeacherId = new Guid("5aaac979-05dd-4e9a-8174-6d4885a40ce7"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },

                    // Nguyễn Trọng Hòa
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"), TeacherId = new Guid("c1f9b122-383b-42bd-9a04-10964679b550"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"), TeacherId = new Guid("c1f9b122-383b-42bd-9a04-10964679b550"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"), TeacherId = new Guid("c1f9b122-383b-42bd-9a04-10964679b550"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Nguyễn Thị Kim Huyền
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"), TeacherId = new Guid("c85fbb73-6a36-4457-9477-0bb332b78d62"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"), TeacherId = new Guid("c85fbb73-6a36-4457-9477-0bb332b78d62"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("c85fbb73-6a36-4457-9477-0bb332b78d62"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("c85fbb73-6a36-4457-9477-0bb332b78d62"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },

                    // Nguyễn Thị Hương
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("a159920c-504f-498a-b225-79844ad120ae"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },

                    // Trương Thị Kỷ
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("a159920c-504f-498a-b225-79844ad120ae"), TeacherId = new Guid("e57f2e3e-6335-4fb4-bb4a-c12a73b5d7cd"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("e57f2e3e-6335-4fb4-bb4a-c12a73b5d7cd"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("a159920c-504f-498a-b225-79844ad120ae"), TeacherId = new Guid("e57f2e3e-6335-4fb4-bb4a-c12a73b5d7cd"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Nguyễn Văn Linh
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"), TeacherId = new Guid("33a1c99f-5f8f-400d-84eb-c2bfe2bf8b1e"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"), TeacherId = new Guid("33a1c99f-5f8f-400d-84eb-c2bfe2bf8b1e"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"), TeacherId = new Guid("33a1c99f-5f8f-400d-84eb-c2bfe2bf8b1e"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"), TeacherId = new Guid("33a1c99f-5f8f-400d-84eb-c2bfe2bf8b1e"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("33a1c99f-5f8f-400d-84eb-c2bfe2bf8b1e"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("33a1c99f-5f8f-400d-84eb-c2bfe2bf8b1e"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },

                    // Tống Mỹ Linh
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"), TeacherId = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("a159920c-504f-498a-b225-79844ad120ae"), TeacherId = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"), TeacherId = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"), TeacherId = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"), TeacherId = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"), TeacherId = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },

                    // Trương Thị Linh
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"), TeacherId = new Guid("475b148d-8c8f-42a3-beae-f81c4f01ae3a"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"), TeacherId = new Guid("475b148d-8c8f-42a3-beae-f81c4f01ae3a"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"), TeacherId = new Guid("475b148d-8c8f-42a3-beae-f81c4f01ae3a"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("475b148d-8c8f-42a3-beae-f81c4f01ae3a"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"), TeacherId = new Guid("475b148d-8c8f-42a3-beae-f81c4f01ae3a"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"), TeacherId = new Guid("475b148d-8c8f-42a3-beae-f81c4f01ae3a"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },

                    // Nguyễn Thị Kim Loan
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"), TeacherId = new Guid("cb45fd82-caf7-49b3-8afa-07ad739cba97"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("cb45fd82-caf7-49b3-8afa-07ad739cba97"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },

                    // Võ Văn Anh Lợi
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"), TeacherId = new Guid("6749a629-6964-43f4-bc38-2173b963413c"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("6749a629-6964-43f4-bc38-2173b963413c"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"), TeacherId = new Guid("6749a629-6964-43f4-bc38-2173b963413c"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("6749a629-6964-43f4-bc38-2173b963413c"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },

                    // Nguyễn Trương Trà My
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"), TeacherId = new Guid("e936ed8e-b9ff-448c-b641-2cede0b5c9da"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"), TeacherId = new Guid("e936ed8e-b9ff-448c-b641-2cede0b5c9da"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"), TeacherId = new Guid("e936ed8e-b9ff-448c-b641-2cede0b5c9da"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"), TeacherId = new Guid("e936ed8e-b9ff-448c-b641-2cede0b5c9da"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Lê Sỹ Nam
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("a90efdce-21e5-4a5c-93fd-d30721738db4"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("a90efdce-21e5-4a5c-93fd-d30721738db4"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("a90efdce-21e5-4a5c-93fd-d30721738db4"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },

                    // Nguyễn Thị Kim Ngân
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },

                    // Nguyễn Thị Bảo Ngọc
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"), TeacherId = new Guid("81324152-45d1-4887-8e48-af431be4a30a"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"), TeacherId = new Guid("81324152-45d1-4887-8e48-af431be4a30a"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("81324152-45d1-4887-8e48-af431be4a30a"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("81324152-45d1-4887-8e48-af431be4a30a"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"), TeacherId = new Guid("81324152-45d1-4887-8e48-af431be4a30a"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("81324152-45d1-4887-8e48-af431be4a30a"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("81324152-45d1-4887-8e48-af431be4a30a"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },

                    // Huỳnh Bác Nhã
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"), TeacherId = new Guid("df6cc373-ae5c-4cce-b000-c48af3fe3e2b"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("df6cc373-ae5c-4cce-b000-c48af3fe3e2b"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },

                    // Ngô Thị Thanh Nhàn
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"), TeacherId = new Guid("29438207-e593-4766-a789-9f60feb184df"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"), TeacherId = new Guid("29438207-e593-4766-a789-9f60feb184df"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"), TeacherId = new Guid("29438207-e593-4766-a789-9f60feb184df"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"), TeacherId = new Guid("29438207-e593-4766-a789-9f60feb184df"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("29438207-e593-4766-a789-9f60feb184df"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },

                    // Nguyễn Như Mỹ Nhân
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"), TeacherId = new Guid("4d72908d-8108-4828-88ad-e1797085b01b"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"), TeacherId = new Guid("4d72908d-8108-4828-88ad-e1797085b01b"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("4d72908d-8108-4828-88ad-e1797085b01b"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("4d72908d-8108-4828-88ad-e1797085b01b"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("4d72908d-8108-4828-88ad-e1797085b01b"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("4d72908d-8108-4828-88ad-e1797085b01b"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },

                    // Nguyễn Huỳnh Nhi
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"), TeacherId = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"), TeacherId = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },

                    // Trần Thị Kim Oanh
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"), TeacherId = new Guid("d3c9cca0-bff7-4fdb-a664-fa55c082c14a"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"), TeacherId = new Guid("d3c9cca0-bff7-4fdb-a664-fa55c082c14a"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"), TeacherId = new Guid("d3c9cca0-bff7-4fdb-a664-fa55c082c14a"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("d3c9cca0-bff7-4fdb-a664-fa55c082c14a"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },

                    // Nguyễn Thành Phụng
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("fb1a4dad-e5a9-416c-aad1-f0c98e394e49"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("fb1a4dad-e5a9-416c-aad1-f0c98e394e49"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("fb1a4dad-e5a9-416c-aad1-f0c98e394e49"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },

                    // Nguyễn Duy Phước
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"), TeacherId = new Guid("d44e77e2-00db-4f28-9364-bab2b01aeb7b"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca") },

                    // Lê Thị Đăng Phương
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"), TeacherId = new Guid("1e514fac-3089-4e09-9bf9-bea005578da5"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("1e514fac-3089-4e09-9bf9-bea005578da5"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("1e514fac-3089-4e09-9bf9-bea005578da5"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("1e514fac-3089-4e09-9bf9-bea005578da5"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("1e514fac-3089-4e09-9bf9-bea005578da5"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("1e514fac-3089-4e09-9bf9-bea005578da5"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("1e514fac-3089-4e09-9bf9-bea005578da5"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },

                    // Võ Thị Thu Phương
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"), TeacherId = new Guid("2cebb584-9d66-4f80-83f1-67197b3dd52a"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"), TeacherId = new Guid("2cebb584-9d66-4f80-83f1-67197b3dd52a"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"), TeacherId = new Guid("2cebb584-9d66-4f80-83f1-67197b3dd52a"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"), TeacherId = new Guid("2cebb584-9d66-4f80-83f1-67197b3dd52a"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("2cebb584-9d66-4f80-83f1-67197b3dd52a"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("2cebb584-9d66-4f80-83f1-67197b3dd52a"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },

                    // Đặng Kim Sơn
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"), TeacherId = new Guid("8a557738-6e34-43c5-9ec9-3b45ac4fdea5"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"), TeacherId = new Guid("8a557738-6e34-43c5-9ec9-3b45ac4fdea5"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("8a557738-6e34-43c5-9ec9-3b45ac4fdea5"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },

                    // Phan Trọng Sơn
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("57064b4d-c018-4d87-a14a-d6412713cdf6"), TeacherId = new Guid("6314c540-22ed-4084-926d-958a0eeedada"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("6314c540-22ed-4084-926d-958a0eeedada"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },

                    // Lê Kim Tám
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"), TeacherId = new Guid("eb146dd1-1439-43b7-b4e2-420e58c733b0"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },

                    // Nguyễn Thái Tân
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"), TeacherId = new Guid("4986a2cc-8233-4a41-93ac-43e35e250c1a"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("4986a2cc-8233-4a41-93ac-43e35e250c1a"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"), TeacherId = new Guid("4986a2cc-8233-4a41-93ac-43e35e250c1a"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Nguyễn Thị Thu Thảo
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("00ab2b62-8c9c-4693-8f3f-b7517f12604e"), TeacherId = new Guid("e0c86ebc-287c-4b8f-8a34-c7752c537ece"), SubjectId = new Guid("93b939d4-ce8f-479c-a82f-ed2fe1278b31") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"), TeacherId = new Guid("e0c86ebc-287c-4b8f-8a34-c7752c537ece"), SubjectId = new Guid("93b939d4-ce8f-479c-a82f-ed2fe1278b31") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"), TeacherId = new Guid("e0c86ebc-287c-4b8f-8a34-c7752c537ece"), SubjectId = new Guid("93b939d4-ce8f-479c-a82f-ed2fe1278b31") },

                    // Nguyễn Thị Hồng Thẩm
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"), TeacherId = new Guid("307eb00b-0f9b-4f40-a955-57b4fe1b60e4"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"), TeacherId = new Guid("307eb00b-0f9b-4f40-a955-57b4fe1b60e4"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"), TeacherId = new Guid("307eb00b-0f9b-4f40-a955-57b4fe1b60e4"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"), TeacherId = new Guid("307eb00b-0f9b-4f40-a955-57b4fe1b60e4"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },

                    // Tô Hữu Thọ
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"), TeacherId = new Guid("83d7b05b-69e5-4e88-8166-28dd81a004d9"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"), TeacherId = new Guid("83d7b05b-69e5-4e88-8166-28dd81a004d9"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"), TeacherId = new Guid("83d7b05b-69e5-4e88-8166-28dd81a004d9"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"), TeacherId = new Guid("83d7b05b-69e5-4e88-8166-28dd81a004d9"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"), TeacherId = new Guid("83d7b05b-69e5-4e88-8166-28dd81a004d9"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("83d7b05b-69e5-4e88-8166-28dd81a004d9"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("83d7b05b-69e5-4e88-8166-28dd81a004d9"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },

                    // Nguyễn Thị Thoa
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"), TeacherId = new Guid("f8fa22ac-0799-4bff-a34c-2674e70f012f"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"), TeacherId = new Guid("f8fa22ac-0799-4bff-a34c-2674e70f012f"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("f8fa22ac-0799-4bff-a34c-2674e70f012f"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("f8fa22ac-0799-4bff-a34c-2674e70f012f"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },

                    // Phan Huy Thùy
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"), TeacherId = new Guid("c27449f8-6eac-439a-901c-9b0b534ab428"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"), TeacherId = new Guid("c27449f8-6eac-439a-901c-9b0b534ab428"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"), TeacherId = new Guid("c27449f8-6eac-439a-901c-9b0b534ab428"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"), TeacherId = new Guid("c27449f8-6eac-439a-901c-9b0b534ab428"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },

                    // Lê Thanh Thủy
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"), TeacherId = new Guid("84d3bc96-0ad1-4269-8b40-207c2469e534"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"), TeacherId = new Guid("84d3bc96-0ad1-4269-8b40-207c2469e534"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("84d3bc96-0ad1-4269-8b40-207c2469e534"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("84d3bc96-0ad1-4269-8b40-207c2469e534"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb") },

                    // Phạm Thị Thúy
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },

                    // Đỗ Thanh Tiến
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"), TeacherId = new Guid("e7dc5f6e-0671-48ec-a298-9bd7ab8ffbf1"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"), TeacherId = new Guid("e7dc5f6e-0671-48ec-a298-9bd7ab8ffbf1"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"), TeacherId = new Guid("e7dc5f6e-0671-48ec-a298-9bd7ab8ffbf1"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Đặng Ngô Toàn
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"), TeacherId = new Guid("14636c24-323b-459f-a824-0413df568c09"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("14636c24-323b-459f-a824-0413df568c09"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("14636c24-323b-459f-a824-0413df568c09"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },

                    // Đặng Thị Huyền Trang
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"), TeacherId = new Guid("5fe80a00-fa41-476c-a915-0238a70672cc"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("5fe80a00-fa41-476c-a915-0238a70672cc"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"), TeacherId = new Guid("5fe80a00-fa41-476c-a915-0238a70672cc"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"), TeacherId = new Guid("5fe80a00-fa41-476c-a915-0238a70672cc"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Nguyễn Thị Kiều Trang
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"), TeacherId = new Guid("7f392915-ee09-4dcf-91ec-17d94ec39563"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("7f392915-ee09-4dcf-91ec-17d94ec39563"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("c637c086-455b-4537-a1e2-988cf0256f0f"), TeacherId = new Guid("7f392915-ee09-4dcf-91ec-17d94ec39563"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Nguyễn Thị Thu Trâm
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"), TeacherId = new Guid("0c6c9e47-e7c5-4d30-9945-65672be86dfc"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"), TeacherId = new Guid("0c6c9e47-e7c5-4d30-9945-65672be86dfc"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("0c6c9e47-e7c5-4d30-9945-65672be86dfc"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87") },

                    // Bùi Thị Huyền Trân
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("e2ce2775-b86a-4990-9d7c-44580df938b5"), TeacherId = new Guid("4b3559b6-42ca-4f25-9192-d54903255462"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("4b3559b6-42ca-4f25-9192-d54903255462"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("4b3559b6-42ca-4f25-9192-d54903255462"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },

                    // Nguyễn Thị Kim Trinh
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("499be294-43cb-4c41-8c83-e2737065ee76"), TeacherId = new Guid("8fdee9a8-c15d-41c0-97c7-36cd9f3a30bf"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("8fdee9a8-c15d-41c0-97c7-36cd9f3a30bf"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e") },

                    // Nguyễn Chí Trường
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("a159920c-504f-498a-b225-79844ad120ae"), TeacherId = new Guid("1a4e58d0-5e19-42c2-b73f-ac0d7b6988f1"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0cb456a0-c788-468f-ba51-103cf6cef5f3"), TeacherId = new Guid("1a4e58d0-5e19-42c2-b73f-ac0d7b6988f1"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },

                    // Đặng Văn Tuấn
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"), TeacherId = new Guid("a1a9f708-9a48-42fd-9308-3a106f9be31f"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"), TeacherId = new Guid("a1a9f708-9a48-42fd-9308-3a106f9be31f"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"), TeacherId = new Guid("a1a9f708-9a48-42fd-9308-3a106f9be31f"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"), TeacherId = new Guid("a1a9f708-9a48-42fd-9308-3a106f9be31f"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Nguyễn Anh Tuấn
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"), TeacherId = new Guid("fbcb8e24-1905-40fb-b49c-d27ff9ce20a2"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"), TeacherId = new Guid("fbcb8e24-1905-40fb-b49c-d27ff9ce20a2"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("86db94ad-fa83-4759-9e04-f0cc617f8952"), TeacherId = new Guid("fbcb8e24-1905-40fb-b49c-d27ff9ce20a2"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },

                    // Phạm Ngọc Tuấn
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("750463ed-ab81-4a4e-8d49-382084f39ad7"), TeacherId = new Guid("d5551b7b-68e6-4b92-a881-c9c57cecb5a8"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1ae1937b-c8fc-4ee7-9641-b5d0909c54aa"), TeacherId = new Guid("d5551b7b-68e6-4b92-a881-c9c57cecb5a8"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("1055c01e-47b0-4314-8141-112d528d82eb"), TeacherId = new Guid("d5551b7b-68e6-4b92-a881-c9c57cecb5a8"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },

                    // Trần Quang Tuyến
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("853a31a6-f92d-450f-9b42-b445bb04cc18"), TeacherId = new Guid("7991b687-afc4-4995-b7ca-60eba94daa52"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("7991b687-afc4-4995-b7ca-60eba94daa52"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("f6123108-6be8-496d-9c60-7753f313368f"), TeacherId = new Guid("7991b687-afc4-4995-b7ca-60eba94daa52"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },

                    // Đỗ Văn Tư
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"), TeacherId = new Guid("fc19384b-21a7-47e8-89cd-f569544c15c1"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("feb2d51a-8ad1-4c18-8449-4e658ad02334"), TeacherId = new Guid("fc19384b-21a7-47e8-89cd-f569544c15c1"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5725d2e-d7d8-4831-a35e-3ca107bed73f"), TeacherId = new Guid("fc19384b-21a7-47e8-89cd-f569544c15c1"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69") },

                    // Phạm Văn Tức
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("d5259f06-9165-450f-9eab-b39c609416d7"), TeacherId = new Guid("cee1fbee-342f-4089-8051-57f71c7d3903"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("bb1c4f4d-a4fa-4d2e-abcb-6bf7164ae2dc"), TeacherId = new Guid("cee1fbee-342f-4089-8051-57f71c7d3903"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("eaeea2ab-abc7-4aba-b48e-c97aa4c8559d"), TeacherId = new Guid("cee1fbee-342f-4089-8051-57f71c7d3903"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("cee1fbee-342f-4089-8051-57f71c7d3903"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("b70f1908-aea3-4303-9d50-a0bfae418459"), TeacherId = new Guid("cee1fbee-342f-4089-8051-57f71c7d3903"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53") },

                    // Nguyễn Thị Lệ Uyên
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"), TeacherId = new Guid("f6a2d0e1-fcdc-4ea0-9c40-ffea09fd9490"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("99dd07b4-0450-4efc-a70a-192d81577ced"), TeacherId = new Guid("f6a2d0e1-fcdc-4ea0-9c40-ffea09fd9490"), SubjectId = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("a159920c-504f-498a-b225-79844ad120ae"), TeacherId = new Guid("f6a2d0e1-fcdc-4ea0-9c40-ffea09fd9490"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },

                    // Phan Thị Lệ Xuân
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"), TeacherId = new Guid("4f2b309b-1b10-4204-b627-7d376533ea15"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ccf8e5a7-4301-447b-8825-3335b520dffc"), TeacherId = new Guid("4f2b309b-1b10-4204-b627-7d376533ea15"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("aebd2111-a72b-41d8-b488-a59cc0a814ae"), TeacherId = new Guid("4f2b309b-1b10-4204-b627-7d376533ea15"), SubjectId = new Guid("c8e0a342-906f-494a-8554-084775b7b760") },

                    // Võ Phú Xuân
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("6a0e3bb6-c513-48f9-b7b9-1208dac116fd"), TeacherId = new Guid("00d40c59-0ba2-4231-9b6a-95345eea58bf"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("0df218c3-e2fa-4896-adc5-be7b4edddd68"), TeacherId = new Guid("00d40c59-0ba2-4231-9b6a-95345eea58bf"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") },
                    new Assignment { Id = Guid.NewGuid(), ClassId = new Guid("ad0fd2a4-e653-47bc-adb3-a70b61e20e3a"), TeacherId = new Guid("00d40c59-0ba2-4231-9b6a-95345eea58bf"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02") }
                );
        }
    }
}
