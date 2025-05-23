using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReturningRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReturningRequests_AcceptedBy",
                table: "ReturningRequests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnedDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "AcceptedBy",
                table: "ReturningRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "ReturningRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReturningRequests_AcceptedBy",
                table: "ReturningRequests",
                column: "AcceptedBy",
                unique: true,
                filter: "[AcceptedBy] IS NOT NULL");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturningRequests_Assignments_AssignmentId",
                table: "ReturningRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReturningRequests_AcceptedBy",
                table: "ReturningRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReturningRequests_AssignmentId",
                table: "ReturningRequests");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "ReturningRequests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnedDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AcceptedBy",
                table: "ReturningRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturningRequests_AcceptedBy",
                table: "ReturningRequests",
                column: "AcceptedBy",
                unique: true);
        }
    }
}
