using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HerdFlow.Api.Migrations;

public partial class ReplacePregnantWithBred : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            UPDATE "Cows"
            SET "PregnancyStatus" = 'Bred'
            WHERE "PregnancyStatus" = 'Pregnant';
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            UPDATE "Cows"
            SET "PregnancyStatus" = 'Pregnant'
            WHERE "PregnancyStatus" = 'Bred';
            """);
    }
}
