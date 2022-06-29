using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerStatResourceServer.Migrations
{
    public partial class RemoveScoreFromMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Score_ScoreId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Score");

            migrationBuilder.DropIndex(
                name: "IX_Matches_ScoreId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ScoreId",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamExtraTime",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamFullTime",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamPenalties",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamExtraTime",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamFullTime",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamPenalties",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamExtraTime",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "AwayTeamFullTime",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "AwayTeamPenalties",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeTeamExtraTime",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeTeamFullTime",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeTeamPenalties",
                table: "Matches");

            migrationBuilder.AddColumn<string>(
                name: "ScoreId",
                table: "Matches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Score",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AwayTeamExtraTime = table.Column<int>(type: "int", nullable: false),
                    AwayTeamFullTime = table.Column<int>(type: "int", nullable: false),
                    AwayTeamPenalties = table.Column<int>(type: "int", nullable: false),
                    HomeTeamExtraTime = table.Column<int>(type: "int", nullable: false),
                    HomeTeamFullTime = table.Column<int>(type: "int", nullable: false),
                    HomeTeamPenalties = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ScoreId",
                table: "Matches",
                column: "ScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Score_ScoreId",
                table: "Matches",
                column: "ScoreId",
                principalTable: "Score",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
