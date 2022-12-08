using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Purchasing_management.Data.Migrations
{
    public partial class createusertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("cab51058-0996-4221-ba63-b841004e89dd"), 0, "ddefe6a5-99de-457c-a262-6a78e8015afd", "ngoxuanhinham123@gmail.com", false, false, null, null, null, "Admin#123", null, null, false, null, false, "NXH2801" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("cab51058-0996-4221-ba63-b841004e89dd"));
        }
    }
}
