using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataVision.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseTableCells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatabaseTableCells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatabaseTableId = table.Column<int>(type: "int", nullable: false),
                    DatabaseTableRowId = table.Column<int>(type: "int", nullable: false),
                    DatabaseTableColumnId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseTableCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatabaseTableCells_DatabaseTableColumns_DatabaseTableColumnId",
                        column: x => x.DatabaseTableColumnId,
                        principalTable: "DatabaseTableColumns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DatabaseTableCells_DatabaseTableRows_DatabaseTableRowId",
                        column: x => x.DatabaseTableRowId,
                        principalTable: "DatabaseTableRows",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DatabaseTableCells_DatabaseTables_DatabaseTableId",
                        column: x => x.DatabaseTableId,
                        principalTable: "DatabaseTables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseTableCells_DatabaseTableColumnId",
                table: "DatabaseTableCells",
                column: "DatabaseTableColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseTableCells_DatabaseTableId",
                table: "DatabaseTableCells",
                column: "DatabaseTableId");

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseTableCells_DatabaseTableRowId",
                table: "DatabaseTableCells",
                column: "DatabaseTableRowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatabaseTableCells");
        }
    }
}
