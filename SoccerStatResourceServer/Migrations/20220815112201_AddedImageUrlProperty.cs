using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerStatResourceServer.Migrations
{
    public partial class AddedImageUrlProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Leagues",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Leagues");
        }
    }
}
