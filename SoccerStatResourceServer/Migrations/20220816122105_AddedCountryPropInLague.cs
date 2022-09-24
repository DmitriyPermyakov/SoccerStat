using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerStatResourceServer.Migrations
{
    public partial class AddedCountryPropInLague : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Leagues",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Leagues");
        }
    }
}
