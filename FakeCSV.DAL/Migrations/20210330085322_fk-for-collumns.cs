using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeCSV.DAL.Migrations
{
    public partial class fkforcollumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Columns_Schemas_SchemaId",
                table: "Columns");

            migrationBuilder.AlterColumn<int>(
                name: "SchemaId",
                table: "Columns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Columns_Schemas_SchemaId",
                table: "Columns",
                column: "SchemaId",
                principalTable: "Schemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Columns_Schemas_SchemaId",
                table: "Columns");

            migrationBuilder.AlterColumn<int>(
                name: "SchemaId",
                table: "Columns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Columns_Schemas_SchemaId",
                table: "Columns",
                column: "SchemaId",
                principalTable: "Schemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
