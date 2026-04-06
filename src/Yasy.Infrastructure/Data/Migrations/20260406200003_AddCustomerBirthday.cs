using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yasy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerBirthday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Birthday",
                table: "customers",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "customers");
        }
    }
}
