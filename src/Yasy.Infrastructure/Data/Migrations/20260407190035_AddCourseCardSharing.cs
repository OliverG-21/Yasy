using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yasy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseCardSharing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "course_card_sharings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CourseCardId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_card_sharings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_course_card_sharings_course_cards_CourseCardId",
                        column: x => x.CourseCardId,
                        principalTable: "course_cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_card_sharings_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_card_sharings_CourseCardId_CustomerId",
                table: "course_card_sharings",
                columns: new[] { "CourseCardId", "CustomerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_card_sharings_CustomerId",
                table: "course_card_sharings",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_card_sharings");
        }
    }
}
