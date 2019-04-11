using Microsoft.EntityFrameworkCore.Migrations;

namespace StrongStart.Data.Migrations
{
    public partial class AddFinishPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinishPart1",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinishPart2",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "isQualify",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishPart1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FinishPart2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "isQualify",
                table: "AspNetUsers");
        }
    }
}
