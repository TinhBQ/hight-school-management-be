using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired(false);
            entity.Property(e => e.Hash).IsRequired(false);
            entity.Property(e => e.Salt).IsRequired(false);
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.MiddleName).IsRequired(false);
            entity.Property(e => e.ShortName).IsRequired();
            entity.Property(e => e.PeriodCount);

            //entity.HasMany(e => e.Classes)
            //    .WithOne(e => e.HomeroomTeacher)
            //    .HasForeignKey<Class>(e => e.HomeroomTeacherId)
            //    .HasConstraintName("FK__Teacher__Class__HomeroomTeacherId")
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .IsRequired(false);

            //entity.HasData
            //    (
            //        new Teacher { Id = new Guid("4ecdfa98-3748-490a-94d9-d41372ede3af"), FirstName = "Đặng", MiddleName = "Thị Hồng", LastName = "Hoa", ShortName = "Hoa(A)" },
            //        new Teacher { Id = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), FirstName = "Nguyễn", MiddleName = "Huỳnh", LastName = "Nhi", ShortName = "Nhi" },
            //        new Teacher { Id = new Guid("17179670-0ac5-417a-b499-55c4bcaeaecc"), FirstName = "Nguyễn", MiddleName = "Thị Lan", LastName = "Anh", ShortName = "Anh" },
            //        new Teacher { Id = new Guid("7991b687-afc4-4995-b7ca-60eba94daa52"), FirstName = "Trần", MiddleName = "Quang", LastName = "Tuyến", ShortName = "Tuyến" },
            //        new Teacher { Id = new Guid("fc19384b-21a7-47e8-89cd-f569544c15c1"), FirstName = "Đỗ", MiddleName = "Văn", LastName = "Tư", ShortName = "Tư" },
            //        new Teacher { Id = new Guid("a1a9f708-9a48-42fd-9308-3a106f9be31f"), FirstName = "Đặng", MiddleName = "Văn", LastName = "Tuấn", ShortName = "Tuấn(S)" },
            //        new Teacher { Id = new Guid("5e80e258-35bd-42b3-a6a5-cd46794565a9"), FirstName = "Nguyễn", MiddleName = "Thị Thanh", LastName = "Hoa", ShortName = "Hoa(M)" },
            //        new Teacher { Id = new Guid("1a4e58d0-5e19-42c2-b73f-ac0d7b6988f1"), FirstName = "Nguyễn", MiddleName = "Chí", LastName = "Trường", ShortName = "Trường" },
            //        new Teacher { Id = new Guid("14636c24-323b-459f-a824-0413df568c09"), FirstName = "Đặng", MiddleName = "Ngô", LastName = "Toàn", ShortName = "Toàn" },
            //        new Teacher { Id = new Guid("8a557738-6e34-43c5-9ec9-3b45ac4fdea5"), FirstName = "Đặng", MiddleName = "Kim", LastName = "Sơn", ShortName = "Sơn(H)" },
            //        new Teacher { Id = new Guid("cb45fd82-caf7-49b3-8afa-07ad739cba97"), FirstName = "Nguyễn", MiddleName = "Thị Kim", LastName = "Loan", ShortName = "Loan" },
            //        new Teacher { Id = new Guid("2cebb584-9d66-4f80-83f1-67197b3dd52a"), FirstName = "Võ", MiddleName = "Thị Thu", LastName = "Phương", ShortName = "Phương(T)" },
            //        new Teacher { Id = new Guid("6314c540-22ed-4084-926d-958a0eeedada"), FirstName = "Phan", MiddleName = "Trọng", LastName = "Sơn", ShortName = "Sơn(S)" },
            //        new Teacher { Id = new Guid("0e53a075-6f6f-472f-a356-ca31d8fbd1a7"), FirstName = "Dương", MiddleName = "Quốc", LastName = "Thuấn", ShortName = "Thuất" },
            //        new Teacher { Id = new Guid("0c6c9e47-e7c5-4d30-9945-65672be86dfc"), FirstName = "Nguyễn", MiddleName = "Thị Thu", LastName = "Trâm", ShortName = "Trâm" },
            //        new Teacher { Id = new Guid("d07509af-9b19-46d4-a79c-018a1c247efb"), FirstName = "Thạch", MiddleName = "Thị Mỹ", LastName = "Nhật", ShortName = "Nhật" },
            //        new Teacher { Id = new Guid("fb1a4dad-e5a9-416c-aad1-f0c98e394e49"), FirstName = "Nguyễn", MiddleName = "Thành", LastName = "Phụng", ShortName = "Phụng" },
            //        new Teacher { Id = new Guid("1e514fac-3089-4e09-9bf9-bea005578da5"), FirstName = "Lê", MiddleName = "Thị Đăng", LastName = "Phương", ShortName = "Phương(L)" },
            //        new Teacher { Id = new Guid("fbcb8e24-1905-40fb-b49c-d27ff9ce20a2"), FirstName = "Nguyễn", MiddleName = "Anh", LastName = "Tuấn", ShortName = "Tuấn(L)" },
            //        new Teacher { Id = new Guid("c27449f8-6eac-439a-901c-9b0b534ab428"), FirstName = "Phan", MiddleName = "Huy", LastName = "Thùy", ShortName = "Thùy" },
            //        new Teacher { Id = new Guid("9f09c5a3-7799-40ed-911c-c5161c05cabc"), FirstName = "Đỗ", MiddleName = "Thị Thu", LastName = "Hằng", ShortName = "Hằng" },
            //        new Teacher { Id = new Guid("6749a629-6964-43f4-bc38-2173b963413c"), FirstName = "Võ", MiddleName = "Văn Anh", LastName = "Lợi", ShortName = "Lợi" },
            //        new Teacher { Id = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), FirstName = "Tống", MiddleName = "Mỹ", LastName = "Linh", ShortName = "Tg.Linh" },
            //        new Teacher { Id = new Guid("84d3bc96-0ad1-4269-8b40-207c2469e534"), FirstName = "Lê", MiddleName = "Thanh", LastName = "Thủy", ShortName = "Thủy" },
            //        new Teacher { Id = new Guid("475b148d-8c8f-42a3-beae-f81c4f01ae3a"), FirstName = "Trương", MiddleName = "Thị", LastName = "Linh", ShortName = "Tr-Linh" },
            //        new Teacher { Id = new Guid("f8fa22ac-0799-4bff-a34c-2674e70f012f"), FirstName = "Nguyễn", MiddleName = "Thị", LastName = "Thoa", ShortName = "Thoa" },
            //        new Teacher { Id = new Guid("647c4458-ab3e-45b5-9b71-7e109bfffdb5"), FirstName = "Đỗ", MiddleName = "Quốc", LastName = "Công", ShortName = "Công" },
            //        new Teacher { Id = new Guid("e57f2e3e-6335-4fb4-bb4a-c12a73b5d7cd"), FirstName = "Trương", MiddleName = "Thị", LastName = "Kỷ", ShortName = "Kỷ" },
            //        new Teacher { Id = new Guid("eb146dd1-1439-43b7-b4e2-420e58c733b0"), FirstName = "Lê", MiddleName = "Kim", LastName = "Tám", ShortName = "Tám" },
            //        new Teacher { Id = new Guid("c85fbb73-6a36-4457-9477-0bb332b78d62"), FirstName = "Nguyễn", MiddleName = "Thị Kim", LastName = "Huyền", ShortName = "Huyền" },
            //        new Teacher { Id = new Guid("c1f9b122-383b-42bd-9a04-10964679b550"), FirstName = "Nguyễn", MiddleName = "Trọng", LastName = "Hòa", ShortName = "Hòa" },
            //        new Teacher { Id = new Guid("77277057-efa2-49b6-84e0-9ba7da852091"), FirstName = "Phan", MiddleName = "Thế", LastName = "Duy", ShortName = "Duy(V)" },
            //        new Teacher { Id = new Guid("a72bad70-0eda-45a1-9842-eb7e09527203"), FirstName = "Trương", MiddleName = "Tấn", LastName = "Lượng ", ShortName = "Lượng" },
            //        new Teacher { Id = new Guid("7f392915-ee09-4dcf-91ec-17d94ec39563"), FirstName = "Nguyễn", MiddleName = "Thị Kiều", LastName = "Trang", ShortName = "Trang(H)" },
            //        new Teacher { Id = new Guid("4b3559b6-42ca-4f25-9192-d54903255462"), FirstName = "Bùi", MiddleName = "Thị Huyền", LastName = "Trân", ShortName = "Trân" },
            //        new Teacher { Id = new Guid("e7dc5f6e-0671-48ec-a298-9bd7ab8ffbf1"), FirstName = "Đỗ", MiddleName = "Thanh", LastName = "Tiến", ShortName = "Tiến" },
            //        new Teacher { Id = new Guid("06583417-ef88-4a7f-a77e-ae2f494e5183"), FirstName = "Võ", MiddleName = "Mạnh", LastName = "Trí ", ShortName = "Trí" },
            //        new Teacher { Id = new Guid("a90efdce-21e5-4a5c-93fd-d30721738db4"), FirstName = "Lê", MiddleName = "Sỹ", LastName = "Nam", ShortName = "Nam" },
            //        new Teacher { Id = new Guid("dae59f4a-9ee2-4995-8034-fd78c18d99bf"), FirstName = "Đỗ", MiddleName = "Đại", LastName = "Duy", ShortName = "Duy(L)" },
            //        new Teacher { Id = new Guid("e889a534-4a57-41f1-8d12-f81358bd2b82"), FirstName = "Phạm", MiddleName = "Thị", LastName = "Đệ", ShortName = "Đệ" },
            //        new Teacher { Id = new Guid("e936ed8e-b9ff-448c-b641-2cede0b5c9da"), FirstName = "Nguyễn", MiddleName = "Trương Trà", LastName = "My", ShortName = "My" },
            //        new Teacher { Id = new Guid("e0c86ebc-287c-4b8f-8a34-c7752c537ece"), FirstName = "Nguyễn", MiddleName = "Thị Thu", LastName = "Thảo", ShortName = "Thảo" },
            //        new Teacher { Id = new Guid("d44e77e2-00db-4f28-9364-bab2b01aeb7b"), FirstName = "Nguyễn", MiddleName = "Duy", LastName = "Phước", ShortName = "Phước" },
            //        new Teacher { Id = new Guid("df6cc373-ae5c-4cce-b000-c48af3fe3e2b"), FirstName = "Huỳnh", MiddleName = "Bác", LastName = "Nhã", ShortName = "Nhã" },
            //        new Teacher { Id = new Guid("83d7b05b-69e5-4e88-8166-28dd81a004d9"), FirstName = "Tô", MiddleName = "Hữu", LastName = "Thọ", ShortName = "Thọ" },
            //        new Teacher { Id = new Guid("cee1fbee-342f-4089-8051-57f71c7d3903"), FirstName = "Phạm", MiddleName = "Văn", LastName = "Tức", ShortName = "Tức" },
            //        new Teacher { Id = new Guid("00d40c59-0ba2-4231-9b6a-95345eea58bf"), FirstName = "Võ", MiddleName = "Phú", LastName = "Xuân", ShortName = "Xuân(T)" },
            //        new Teacher { Id = new Guid("5aaac979-05dd-4e9a-8174-6d4885a40ce7"), FirstName = "Nguyễn", MiddleName = "Thị Thanh", LastName = "Hoa", ShortName = "Hoa(V)" },
            //        new Teacher { Id = new Guid("67174939-7dbf-44b7-a94d-42f3cc73457a"), FirstName = "Bùi", MiddleName = "Thị Ngọc", LastName = "Cẩm", ShortName = "Cẩm" },
            //        new Teacher { Id = new Guid("f6a2d0e1-fcdc-4ea0-9c40-ffea09fd9490"), FirstName = "Nguyễn", MiddleName = "Thị Lệ", LastName = "Uyên", ShortName = "Uyên" },
            //        new Teacher { Id = new Guid("4bebc80b-f85d-42f6-910a-7c196354ee44"), FirstName = "Đồng", MiddleName = "Xuân", LastName = "Khánh ", ShortName = "Khánh" },
            //        new Teacher { Id = new Guid("33a1c99f-5f8f-400d-84eb-c2bfe2bf8b1e"), FirstName = "Nguyễn", MiddleName = "Văn", LastName = "Linh", ShortName = "Linh(Đ)" },
            //        new Teacher { Id = new Guid("307eb00b-0f9b-4f40-a955-57b4fe1b60e4"), FirstName = "Nguyễn", MiddleName = "Thị Hồng", LastName = "Thẩm", ShortName = "Thẩm" },
            //        new Teacher { Id = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), FirstName = "Nguyễn", MiddleName = "Thị", LastName = "Hương", ShortName = "Hương" },
            //        new Teacher { Id = new Guid("4986a2cc-8233-4a41-93ac-43e35e250c1a"), FirstName = "Nguyễn", MiddleName = "Thái", LastName = "Tân", ShortName = "Tân" },
            //        new Teacher { Id = new Guid("81324152-45d1-4887-8e48-af431be4a30a"), FirstName = "Nguyễn", MiddleName = "Thị Bảo", LastName = "Ngọc", ShortName = "Ngọc" },
            //        new Teacher { Id = new Guid("d5551b7b-68e6-4b92-a881-c9c57cecb5a8"), FirstName = "Phạm", MiddleName = "Ngọc", LastName = "Tuấn", ShortName = "Tuấn(T)" },
            //        new Teacher { Id = new Guid("8fdee9a8-c15d-41c0-97c7-36cd9f3a30bf"), FirstName = "Nguyễn", MiddleName = "Thị Kim", LastName = "Trinh", ShortName = "Trinh" },
            //        new Teacher { Id = new Guid("bad1942a-795d-451e-802f-ce950719969c"), FirstName = "Phạm", MiddleName = "Thị", LastName = "Thúy", ShortName = "Thúy" },
            //        new Teacher { Id = new Guid("2de02504-74a4-4e8d-8100-81ece22d7db2"), FirstName = "Trương", MiddleName = "Văn", LastName = "Kim", ShortName = "Kim" },
            //        new Teacher { Id = new Guid("4f2b309b-1b10-4204-b627-7d376533ea15"), FirstName = "Phan", MiddleName = "Thị Lệ", LastName = "Xuân", ShortName = "Xuân(S)" },
            //        new Teacher { Id = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), FirstName = "Nguyễn", MiddleName = "Thị Kim", LastName = "Ngân", ShortName = "Ngân" },
            //        new Teacher { Id = new Guid("4d72908d-8108-4828-88ad-e1797085b01b"), FirstName = "Nguyễn", MiddleName = "Như Mỹ", LastName = "Nhân", ShortName = "Nhân" },
            //        new Teacher { Id = new Guid("b7adecee-f4d7-4a2d-8457-e5d27a427278"), FirstName = "Lê", MiddleName = "Thị Ngọc", LastName = "Ánh", ShortName = "Ánh" },
            //        new Teacher { Id = new Guid("5fe80a00-fa41-476c-a915-0238a70672cc"), FirstName = "Đặng", MiddleName = "Thị Huyền", LastName = "Trang", ShortName = "Trang(S)" },
            //        new Teacher { Id = new Guid("d3c9cca0-bff7-4fdb-a664-fa55c082c14a"), FirstName = "Trần", MiddleName = "Thị Kim", LastName = "Oanh", ShortName = "Oanh" },
            //        new Teacher { Id = new Guid("470ef95a-4b76-456f-8882-291493be8355"), FirstName = "Phạm", MiddleName = "Như", LastName = "Phục", ShortName = "Phục" },
            //        new Teacher { Id = new Guid("29438207-e593-4766-a789-9f60feb184df"), FirstName = "Ngô", MiddleName = "Thị Thanh", LastName = "Nhàn", ShortName = "Nhàn" },
            //        new Teacher { Id = new Guid("c750afe2-d099-4ead-ba1b-691261b75d32"), FirstName = "Bùi", MiddleName = "Thị Kiều", LastName = "Diễm", ShortName = "Diễm" }
            //    );
        }
    }
}
