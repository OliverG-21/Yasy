using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yasy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerInitialRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "InitialRegistration",
                table: "customers",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialRegistration",
                table: "customers");
        }
    }
}
