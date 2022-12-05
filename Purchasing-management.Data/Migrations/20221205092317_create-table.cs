using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Purchasing_management.Data.Migrations
{
    public partial class createtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrdinalNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistantName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrdinalNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplies_PurchaseOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedDate", "Manager", "Name", "OrdinalNumber" },
                values: new object[,]
                {
                    { new Guid("6b1eea43-5597-45a6-bdea-e68c60564247"), new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyễn Tiến Sơn", "Ban Điều Hành", 1 },
                    { new Guid("a052a63d-fa53-44d5-a197-83089818a676"), new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ngô Xuân Hinh", "Ban Đào Tạo", 2 },
                    { new Guid("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"), new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lê Tuấn Việt", "Ban Nhân Sự", 3 },
                    { new Guid("8e2f0a16-4c09-44c7-ba56-8dc62dfd792d"), new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đỗ Tài Linh", "Ban Truyền Thông", 4 },
                    { new Guid("cab51058-0996-4221-ba63-b841004e89dd"), new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàng Minh Chí", "Ban Giám Sát", 5 }
                });

            migrationBuilder.InsertData(
                table: "PurchaseOrders",
                columns: new[] { "Id", "DepartmentId", "RegistantName" },
                values: new object[] { new Guid("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"), new Guid("a052a63d-fa53-44d5-a197-83089818a676"), "Bùi Gia Huy" });

            migrationBuilder.InsertData(
                table: "PurchaseOrders",
                columns: new[] { "Id", "DepartmentId", "RegistantName" },
                values: new object[] { new Guid("6b1eea43-5597-45a6-bdea-e68c60564247"), new Guid("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"), "Lê Tuấn Việt" });

            migrationBuilder.InsertData(
                table: "PurchaseOrders",
                columns: new[] { "Id", "DepartmentId", "RegistantName" },
                values: new object[] { new Guid("a052a63d-fa53-44d5-a197-83089818a676"), new Guid("8e2f0a16-4c09-44c7-ba56-8dc62dfd792d"), "Đỗ Tài Linh" });

            migrationBuilder.InsertData(
                table: "Supplies",
                columns: new[] { "Id", "Amount", "Name", "OrderId", "OrdinalNumber" },
                values: new object[,]
                {
                    { new Guid("6b1eea43-5597-45a6-bdea-e68c60564247"), "3", "Máy tính", new Guid("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"), 1 },
                    { new Guid("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"), "2", "Chậu hoa", new Guid("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"), 2 },
                    { new Guid("cab51058-0996-4221-ba63-b841004e89dd"), "2", "Bàn", new Guid("6b1eea43-5597-45a6-bdea-e68c60564247"), 3 },
                    { new Guid("8e2f0a16-4c09-44c7-ba56-8dc62dfd792d"), "2", "Bàn", new Guid("a052a63d-fa53-44d5-a197-83089818a676"), 3 },
                    { new Guid("a052a63d-fa53-44d5-a197-83089818a676"), "6", "Ghế", new Guid("a052a63d-fa53-44d5-a197-83089818a676"), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_DepartmentId",
                table: "PurchaseOrders",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_OrderId",
                table: "Supplies",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
