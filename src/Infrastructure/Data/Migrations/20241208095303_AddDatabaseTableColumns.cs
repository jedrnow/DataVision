using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataVision.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseTables_Databases_DatabaseId",
                table: "DatabaseTables");

            migrationBuilder.CreateTable(
                name: "DatabaseTableColumns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    DatabaseTableId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseTableColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatabaseTableColumns_DatabaseTables_DatabaseTableId",
                        column: x => x.DatabaseTableId,
                        principalTable: "DatabaseTables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseTableColumns_DatabaseTableId",
                table: "DatabaseTableColumns",
                column: "DatabaseTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseTables_Databases_DatabaseId",
                table: "DatabaseTables",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseTables_Databases_DatabaseId",
                table: "DatabaseTables");

            migrationBuilder.DropTable(
                name: "DatabaseTableColumns");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseTables_Databases_DatabaseId",
                table: "DatabaseTables",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
