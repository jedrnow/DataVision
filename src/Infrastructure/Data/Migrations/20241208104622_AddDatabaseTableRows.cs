using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataVision.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseTableRows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatabaseTableRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatabaseTableId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseTableRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatabaseTableRows_DatabaseTables_DatabaseTableId",
                        column: x => x.DatabaseTableId,
                        principalTable: "DatabaseTables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseTableRows_DatabaseTableId",
                table: "DatabaseTableRows",
                column: "DatabaseTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatabaseTableRows");
        }
    }
}
