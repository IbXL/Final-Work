using Microsoft.EntityFrameworkCore.Migrations;

namespace TLH.Migrations
{
    public partial class PTsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSub",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Players");

            migrationBuilder.AddColumn<bool>(
                name: "IsSub",
                table: "PTs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "PTs",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSub",
                table: "PTs");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "PTs");

            migrationBuilder.AddColumn<bool>(
                name: "IsSub",
                table: "Players",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Players",
                type: "text",
                nullable: true);
        }
    }
}
