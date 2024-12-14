using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataVision.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToBackgroundJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DatabaseId",
                table: "BackgroundJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "BackgroundJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatabaseId",
                table: "BackgroundJobs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "BackgroundJobs");
        }
    }
}
