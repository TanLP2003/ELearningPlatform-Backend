using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitLearningServiceDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnrolledCourses",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletionPercentage = table.Column<float>(type: "real", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrolledCourses", x => new { x.UserId, x.CourseId });
                });

            migrationBuilder.CreateTable(
                name: "CourseReviews",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ReviewText = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseReviews", x => new { x.UserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CourseReviews_EnrolledCourses_UserId_CourseId",
                        columns: x => new { x.UserId, x.CourseId },
                        principalTable: "EnrolledCourses",
                        principalColumns: new[] { "UserId", "CourseId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningNotes",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteAt = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningNotes", x => new { x.UserId, x.CourseId, x.LectureId, x.NoteAt });
                    table.ForeignKey(
                        name: "FK_LearningNotes_EnrolledCourses_UserId_CourseId",
                        columns: x => new { x.UserId, x.CourseId },
                        principalTable: "EnrolledCourses",
                        principalColumns: new[] { "UserId", "CourseId" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseReviews");

            migrationBuilder.DropTable(
                name: "LearningNotes");

            migrationBuilder.DropTable(
                name: "EnrolledCourses");
        }
    }
}
