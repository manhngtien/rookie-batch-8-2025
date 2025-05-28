using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Assignment_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturningRequests_Assignments_AssignmentId",
                table: "ReturningRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReturningRequests_AssignmentId",
                table: "ReturningRequests");

            migrationBuilder.AddColumn<int>(
                name: "ReturningRequestId",
                table: "Assignments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ReturningRequestId",
                table: "Assignments",
                column: "ReturningRequestId",
                unique: true,
                filter: "[ReturningRequestId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_ReturningRequests_ReturningRequestId",
                table: "Assignments",
                column: "ReturningRequestId",
                principalTable: "ReturningRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_ReturningRequests_ReturningRequestId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_ReturningRequestId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ReturningRequestId",
                table: "Assignments");

            migrationBuilder.CreateIndex(
                name: "IX_ReturningRequests_AssignmentId",
                table: "ReturningRequests",
                column: "AssignmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturningRequests_Assignments_AssignmentId",
                table: "ReturningRequests",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
