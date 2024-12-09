using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataVision.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDbIdsAddJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DatabaseId",
                table: "DatabaseTableRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DatabaseId",
                table: "DatabaseTableColumns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DatabaseId",
                table: "DatabaseTableCells",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalJobId = table.Column<long>(type: "bigint", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSucceeded = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseTableRows_DatabaseId",
                table: "DatabaseTableRows",
                column: "DatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseTableColumns_DatabaseId",
                table: "DatabaseTableColumns",
                column: "DatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseTableCells_DatabaseId",
                table: "DatabaseTableCells",
                column: "DatabaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseTableCells_Databases_DatabaseId",
                table: "DatabaseTableCells",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseTableColumns_Databases_DatabaseId",
                table: "DatabaseTableColumns",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseTableRows_Databases_DatabaseId",
                table: "DatabaseTableRows",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseTableCells_Databases_DatabaseId",
                table: "DatabaseTableCells");

            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseTableColumns_Databases_DatabaseId",
                table: "DatabaseTableColumns");

            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseTableRows_Databases_DatabaseId",
                table: "DatabaseTableRows");

            migrationBuilder.DropTable(
                name: "BackgroundJobs");

            migrationBuilder.DropIndex(
                name: "IX_DatabaseTableRows_DatabaseId",
                table: "DatabaseTableRows");

            migrationBuilder.DropIndex(
                name: "IX_DatabaseTableColumns_DatabaseId",
                table: "DatabaseTableColumns");

            migrationBuilder.DropIndex(
                name: "IX_DatabaseTableCells_DatabaseId",
                table: "DatabaseTableCells");

            migrationBuilder.DropColumn(
                name: "DatabaseId",
                table: "DatabaseTableRows");

            migrationBuilder.DropColumn(
                name: "DatabaseId",
                table: "DatabaseTableColumns");

            migrationBuilder.DropColumn(
                name: "DatabaseId",
                table: "DatabaseTableCells");
        }
    }
}
