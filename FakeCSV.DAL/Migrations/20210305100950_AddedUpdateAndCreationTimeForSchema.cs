using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeCSV.DAL.Migrations
{
    public partial class AddedUpdateAndCreationTimeForSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Schemas",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Schemas",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Schemas");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Schemas");
        }
    }
}
