using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yasy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCardUsageSharing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsedByCustomerId",
                table: "card_usages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_card_usages_UsedByCustomerId",
                table: "card_usages",
                column: "UsedByCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_card_usages_customers_UsedByCustomerId",
                table: "card_usages",
                column: "UsedByCustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_card_usages_customers_UsedByCustomerId",
                table: "card_usages");

            migrationBuilder.DropIndex(
                name: "IX_card_usages_UsedByCustomerId",
                table: "card_usages");

            migrationBuilder.DropColumn(
                name: "UsedByCustomerId",
                table: "card_usages");
        }
    }
}
