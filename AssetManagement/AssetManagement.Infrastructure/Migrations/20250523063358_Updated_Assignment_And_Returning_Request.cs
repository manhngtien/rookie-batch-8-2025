using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Assignment_And_Returning_Request : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReturningRequests_AcceptedBy",
                table: "ReturningRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReturningRequests_RequestedBy",
                table: "ReturningRequests");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignedBy",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignedTo",
                table: "Assignments");

            migrationBuilder.CreateIndex(
                name: "IX_ReturningRequests_AcceptedBy",
                table: "ReturningRequests",
                column: "AcceptedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturningRequests_RequestedBy",
                table: "ReturningRequests",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedBy",
                table: "Assignments",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedTo",
                table: "Assignments",
                column: "AssignedTo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReturningRequests_AcceptedBy",
                table: "ReturningRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReturningRequests_RequestedBy",
                table: "ReturningRequests");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignedBy",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignedTo",
                table: "Assignments");

            migrationBuilder.CreateIndex(
                name: "IX_ReturningRequests_AcceptedBy",
                table: "ReturningRequests",
                column: "AcceptedBy",
                unique: true,
                filter: "[AcceptedBy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReturningRequests_RequestedBy",
                table: "ReturningRequests",
                column: "RequestedBy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedBy",
                table: "Assignments",
                column: "AssignedBy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedTo",
                table: "Assignments",
                column: "AssignedTo",
                unique: true);
        }
    }
}
