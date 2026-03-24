using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HerdFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagNumber = table.Column<string>(type: "text", nullable: false),
                    OwnerName = table.Column<string>(type: "text", nullable: false),
                    LivestockGroup = table.Column<int>(type: "integer", nullable: false),
                    Sex = table.Column<string>(type: "text", nullable: true),
                    Breed = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    HealthStatus = table.Column<string>(type: "text", nullable: false),
                    HeatStatus = table.Column<int>(type: "integer", nullable: false),
                    BreedingStatus = table.Column<string>(type: "text", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric", nullable: true),
                    SalePrice = table.Column<decimal>(type: "numeric", nullable: true),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SaleDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsRemoved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CowId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Cows_CowId",
                        column: x => x.CowId,
                        principalTable: "Cows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cows_TagNumber",
                table: "Cows",
                column: "TagNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CowId",
                table: "Notes",
                column: "CowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Cows");
        }
    }
}
