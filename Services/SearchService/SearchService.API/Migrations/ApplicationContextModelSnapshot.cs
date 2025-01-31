﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SearchService.API.Infrastructure;

#nullable disable

namespace SearchService.API.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SearchService.API.Entities.SearchCourse", b =>
                {
                    b.Property<Guid>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CourseImage")
                        .HasColumnType("text");

                    b.Property<string>("CourseTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InstructorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SearchCount")
                        .HasColumnType("integer");

                    b.HasKey("CourseId");

                    b.ToTable("SearchCourses");
                });

            modelBuilder.Entity("SearchService.API.Entities.SearchInstructor", b =>
                {
                    b.Property<Guid>("InstructorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<string>("InstructorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SearchCount")
                        .HasColumnType("integer");

                    b.HasKey("InstructorId");

                    b.ToTable("SearchInstructors");
                });
#pragma warning restore 612, 618
        }
    }
}
