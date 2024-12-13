using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataVision.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseTableCells_DatabaseTableColumns_DatabaseTableColumnId",
                table: "DatabaseTableCells");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseTableCells_DatabaseTableColumns_DatabaseTableColumnId",
                table: "DatabaseTableCells",
                column: "DatabaseTableColumnId",
                principalTable: "DatabaseTableColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseTableCells_DatabaseTableColumns_DatabaseTableColumnId",
                table: "DatabaseTableCells");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseTableCells_DatabaseTableColumns_DatabaseTableColumnId",
                table: "DatabaseTableCells",
                column: "DatabaseTableColumnId",
                principalTable: "DatabaseTableColumns",
                principalColumn: "Id");
        }
    }
}
