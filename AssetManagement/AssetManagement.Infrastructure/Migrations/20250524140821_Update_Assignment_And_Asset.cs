using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Assignment_And_Asset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssetCode",
                table: "Assignments");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssetCode",
                table: "Assignments",
                column: "AssetCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssetCode",
                table: "Assignments");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssetCode",
                table: "Assignments",
                column: "AssetCode",
                unique: true);
        }
    }
}
