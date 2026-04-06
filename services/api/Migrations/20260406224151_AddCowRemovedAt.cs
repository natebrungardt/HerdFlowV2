using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HerdFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCowRemovedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "Cows",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "Cows");
        }
    }
}
