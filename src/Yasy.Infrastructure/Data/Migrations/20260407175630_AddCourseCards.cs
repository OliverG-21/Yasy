using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yasy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "course_cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TotalVisits = table.Column<int>(type: "integer", nullable: false),
                    UsedVisits = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PricePaid = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    PurchasedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    ExhaustedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_course_cards_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "card_usages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CourseCardId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_usages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_card_usages_course_cards_CourseCardId",
                        column: x => x.CourseCardId,
                        principalTable: "course_cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_card_usages_courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_usages_CourseCardId",
                table: "card_usages",
                column: "CourseCardId");

            migrationBuilder.CreateIndex(
                name: "IX_card_usages_CourseId",
                table: "card_usages",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_cards_CustomerId",
                table: "course_cards",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card_usages");

            migrationBuilder.DropTable(
                name: "course_cards");
        }
    }
}
