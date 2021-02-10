using Microsoft.EntityFrameworkCore.Migrations;

namespace APICore.Data.Migrations
{
    public partial class RemovedUnnecessaryFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Device",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Log");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "Log",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Log",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
