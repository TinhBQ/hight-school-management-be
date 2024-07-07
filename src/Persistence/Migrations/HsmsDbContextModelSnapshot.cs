﻿// <auto-generated />
using System;
using Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(HsmsDbContext))]
    partial class HsmsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.DAOs.Assignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("EndYear")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("PeriodCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("SchoolShift")
                        .HasColumnType("int");

                    b.Property<int>("Semester")
                        .HasColumnType("int");

                    b.Property<int>("StartYear")
                        .HasColumnType("int");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("Entities.DAOs.Class", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("EndYear")
                        .HasColumnType("int");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<Guid?>("HomeroomTeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PeriodCount")
                        .HasColumnType("int");

                    b.Property<int>("SchoolShift")
                        .HasColumnType("int");

                    b.Property<int>("StartYear")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("HomeroomTeacherId")
                        .IsUnique()
                        .HasFilter("[HomeroomTeacherId] IS NOT NULL");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("Entities.DAOs.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Entities.DAOs.SubjectClass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("PeriodCount")
                        .HasColumnType("int");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("SubjectId");

                    b.ToTable("SubjectClasses");
                });

            modelBuilder.Entity("Entities.DAOs.SubjectTeacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMain")
                        .HasColumnType("bit");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("SubjectTeachers");
                });

            modelBuilder.Entity("Entities.DAOs.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PeriodCount")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("Entities.DAOs.Timetable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("EndYear")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Parameters")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Semester")
                        .HasColumnType("int");

                    b.Property<int>("StartYear")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Timetables");
                });

            modelBuilder.Entity("Entities.DAOs.TimetableUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("StartAt")
                        .HasColumnType("int");

                    b.Property<Guid?>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SubjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TeacherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TimetableId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TimetableId");

                    b.ToTable("TimetablesUnits");
                });

            modelBuilder.Entity("Entities.DAOs.Assignment", b =>
                {
                    b.HasOne("Entities.DAOs.Class", "Class")
                        .WithMany("Assignments")
                        .HasForeignKey("ClassId")
                        .IsRequired()
                        .HasConstraintName("FK__Assignment__Class__ClassId");

                    b.HasOne("Entities.DAOs.Subject", "Subject")
                        .WithMany("Assignments")
                        .HasForeignKey("SubjectId")
                        .IsRequired()
                        .HasConstraintName("FK__Assignment__Subject__SubjectId");

                    b.HasOne("Entities.DAOs.Teacher", "Teacher")
                        .WithMany("Assignments")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("FK__Assignment__Teacher__TeacherId");

                    b.Navigation("Class");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Entities.DAOs.Class", b =>
                {
                    b.HasOne("Entities.DAOs.Teacher", "HomeroomTeacher")
                        .WithOne("Class")
                        .HasForeignKey("Entities.DAOs.Class", "HomeroomTeacherId")
                        .HasConstraintName("FK__Teacher__Class__HomeroomTeacherId");

                    b.Navigation("HomeroomTeacher");
                });

            modelBuilder.Entity("Entities.DAOs.SubjectClass", b =>
                {
                    b.HasOne("Entities.DAOs.Class", "Class")
                        .WithMany("SubjectClasses")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__SubjectClass__Class__ClassId");

                    b.HasOne("Entities.DAOs.Subject", "Subject")
                        .WithMany("SubjectClasses")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__SubjectClass__Subject__SubjectId");

                    b.Navigation("Class");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Entities.DAOs.SubjectTeacher", b =>
                {
                    b.HasOne("Entities.DAOs.Subject", "Subject")
                        .WithMany("SubjectTeachers")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__SubjectTeacher__Subject__SubjectId");

                    b.HasOne("Entities.DAOs.Teacher", "Teacher")
                        .WithMany("SubjectTeachers")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__SubjectTeacher__Teacher__TeacherId");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Entities.DAOs.TimetableUnit", b =>
                {
                    b.HasOne("Entities.DAOs.Timetable", "Timetable")
                        .WithMany("TimetableUnits")
                        .HasForeignKey("TimetableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__TimetableUnit__Timetable__TimetableId");

                    b.Navigation("Timetable");
                });

            modelBuilder.Entity("Entities.DAOs.Class", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("SubjectClasses");
                });

            modelBuilder.Entity("Entities.DAOs.Subject", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("SubjectClasses");

                    b.Navigation("SubjectTeachers");
                });

            modelBuilder.Entity("Entities.DAOs.Teacher", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Class");

                    b.Navigation("SubjectTeachers");
                });

            modelBuilder.Entity("Entities.DAOs.Timetable", b =>
                {
                    b.Navigation("TimetableUnits");
                });
#pragma warning restore 612, 618
        }
    }
}
