using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.ShortName).IsRequired();

            //entity.HasData
            //    (
            //        new Subject { Id = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), Name = "Ngữ văn", ShortName = "VAN" },
            //        new Subject { Id = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb"), Name = "Ngoại ngữ", ShortName = "NN" },
            //        new Subject { Id = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), Name = "Toán học", ShortName = "TOAN" },
            //        new Subject { Id = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd"), Name = "Lịch sử", ShortName = "SU" },
            //        new Subject { Id = new Guid("36dc8e92-f9e5-4a1c-9b7a-11d5c1bef3f1"), Name = "Giáo dục thể chất", ShortName = "TD" },
            //        new Subject { Id = new Guid("b4e8011a-b071-4969-aef5-26a885a59605"), Name = "Giáo dục quốc phòng và an ninh", ShortName = "QP" },
            //        new Subject { Id = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02"), Name = "Tin học", ShortName = "TIN" },
            //        new Subject { Id = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca"), Name = "Địa lý", ShortName = "DIA" },
            //        new Subject { Id = new Guid("c937c26a-de6b-4eb5-a9f3-58c3c37e17db"), Name = "Mĩ thuật", ShortName = "MT" },
            //        new Subject { Id = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e"), Name = "Hóa học", ShortName = "HOA" },
            //        new Subject { Id = new Guid("93b939d4-ce8f-479c-a82f-ed2fe1278b31"), Name = "Âm nhạc", ShortName = "NHAC" },
            //        new Subject { Id = new Guid("ebb67e3f-fdbd-430f-9441-64c0c7ceda9a"), Name = "Chào cờ", ShortName = "CC" },
            //        new Subject { Id = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53"), Name = "Giáo dục công dân", ShortName = "GDCD" },
            //        new Subject { Id = new Guid("8a456850-7bd9-45bf-94ae-fe7249d966a4"), Name = "Giáo dục kinh tế và pháp luật", ShortName = "KT&PL" },
            //        new Subject { Id = new Guid("c8e0a342-906f-494a-8554-084775b7b760"), Name = "Công nghệ", ShortName = "CN" },
            //        new Subject { Id = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba"), Name = "Sinh học", ShortName = "SINH" },
            //        new Subject { Id = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632"), Name = "Vật lý", ShortName = "LY" },
            //        new Subject { Id = new Guid("37a192e0-d40f-4781-95c2-3b86707d026e"), Name = "Hoạt động trải nghiệm, hướng nghiệp", ShortName = "TN&HN" },
            //        new Subject { Id = new Guid("2e66193f-1903-49e9-a575-383024ede8d8"), Name = "Sinh hoạt", ShortName = "SH" }
            //    );
        }
    }
}
