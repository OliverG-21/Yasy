using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yasy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_course_enrollments_CustomerId_CourseId",
                table: "course_enrollments");

            migrationBuilder.AddColumn<DateOnly>(
                name: "UnenrolledAt",
                table: "course_enrollments",
                type: "date",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_enrollments_CustomerId_CourseId_EnrolledAt",
                table: "course_enrollments",
                columns: new[] { "CustomerId", "CourseId", "EnrolledAt" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_course_enrollments_CustomerId_CourseId_EnrolledAt",
                table: "course_enrollments");

            migrationBuilder.DropColumn(
                name: "UnenrolledAt",
                table: "course_enrollments");

            migrationBuilder.CreateIndex(
                name: "IX_course_enrollments_CustomerId_CourseId",
                table: "course_enrollments",
                columns: new[] { "CustomerId", "CourseId" },
                unique: true);
        }
    }
}
