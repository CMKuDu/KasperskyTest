using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestTelcoHub.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovalCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0482d420-8d9b-4828-a2a5-a9504bd97b33");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a83d6db7-499e-47dc-822c-54aca0b06ea1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7444c95-43b4-4bbd-bea9-7ec518b46376");

            migrationBuilder.CreateTable(
                name: "ApprovalCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalCodes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "555770f3-2df6-4b0a-aa6e-dac82a22d985", "1", "Admin", "Admin" },
                    { "a087b04d-9d24-4c93-987f-65712a069856", "2", "Customer", "Customer" },
                    { "e10e9cc1-9d2a-4e84-a92e-ec107c8a0aa0", "3", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalCodes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "555770f3-2df6-4b0a-aa6e-dac82a22d985");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a087b04d-9d24-4c93-987f-65712a069856");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e10e9cc1-9d2a-4e84-a92e-ec107c8a0aa0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0482d420-8d9b-4828-a2a5-a9504bd97b33", "3", "User", "User" },
                    { "a83d6db7-499e-47dc-822c-54aca0b06ea1", "2", "Customer", "Customer" },
                    { "f7444c95-43b4-4bbd-bea9-7ec518b46376", "1", "Admin", "Admin" }
                });
        }
    }
}
