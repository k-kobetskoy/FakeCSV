using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeCSV.DAL.Migrations
{
    public partial class DataSetAddedRowsNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RowsNumber",
                table: "DataSets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowsNumber",
                table: "DataSets");
        }
    }
}
