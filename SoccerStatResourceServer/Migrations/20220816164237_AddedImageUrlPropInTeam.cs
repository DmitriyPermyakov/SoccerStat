using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerStatResourceServer.Migrations
{
    public partial class AddedImageUrlPropInTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Teams");
        }
    }
}
