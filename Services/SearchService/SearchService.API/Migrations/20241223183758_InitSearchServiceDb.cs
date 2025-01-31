using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitSearchServiceDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchCourses",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseTitle = table.Column<string>(type: "text", nullable: false),
                    CourseImage = table.Column<string>(type: "text", nullable: true),
                    InstructorName = table.Column<string>(type: "text", nullable: false),
                    SearchCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchCourses", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "SearchInstructors",
                columns: table => new
                {
                    InstructorId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstructorName = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    SearchCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchInstructors", x => x.InstructorId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchCourses");

            migrationBuilder.DropTable(
                name: "SearchInstructors");
        }
    }
}
