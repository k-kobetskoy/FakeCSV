using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeCSV.DAL.Migrations
{
    public partial class addedfkeyforDataSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSets_Schemas_SchemaId",
                table: "DataSets");

            migrationBuilder.AlterColumn<int>(
                name: "SchemaId",
                table: "DataSets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DataSets_Schemas_SchemaId",
                table: "DataSets",
                column: "SchemaId",
                principalTable: "Schemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSets_Schemas_SchemaId",
                table: "DataSets");

            migrationBuilder.AlterColumn<int>(
                name: "SchemaId",
                table: "DataSets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSets_Schemas_SchemaId",
                table: "DataSets",
                column: "SchemaId",
                principalTable: "Schemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
