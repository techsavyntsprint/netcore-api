using Microsoft.EntityFrameworkCore.Migrations;

namespace APICore.Data.Migrations
{
    public partial class NewFieldsLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "App",
                table: "Log",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "Log",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Log",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Module",
                table: "Log",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "App",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Module",
                table: "Log");
        }
    }
}
