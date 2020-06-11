using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace apbd_tut13.Migrations
{
    public partial class AllShopTablesAddedWithSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Confectionery",
                columns: table => new
                {
                    IdConfectionery = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    PricePerItem = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Confectionery_pk", x => x.IdConfectionery);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    IdClient = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Surname = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Customer_pk", x => x.IdClient);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    IdEmployee = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Surname = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Employee_pk", x => x.IdEmployee);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    IdOrder = table.Column<int>(nullable: false),
                    DateAccepted = table.Column<DateTime>(type: "date", nullable: false),
                    DateFinished = table.Column<DateTime>(type: "date", nullable: false),
                    Notes = table.Column<string>(maxLength: 255, nullable: true),
                    IdClient = table.Column<int>(nullable: false),
                    IdEmployee = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Order_pk", x => x.IdOrder);
                    table.ForeignKey(
                        name: "Order_Customer",
                        column: x => x.IdClient,
                        principalTable: "Customer",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Order_Employee",
                        column: x => x.IdEmployee,
                        principalTable: "Employee",
                        principalColumn: "IdEmployee",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Confectionery_Order",
                columns: table => new
                {
                    IdConfectionery = table.Column<int>(nullable: false),
                    IdOrder = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Confectionery_Order_pk", x => new { x.IdConfectionery, x.IdOrder });
                    table.ForeignKey(
                        name: "Confectionery_Order_Confectionery",
                        column: x => x.IdConfectionery,
                        principalTable: "Confectionery",
                        principalColumn: "IdConfectionery",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Confectionery_Order_Order",
                        column: x => x.IdOrder,
                        principalTable: "Order",
                        principalColumn: "IdOrder",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Confectionery",
                columns: new[] { "IdConfectionery", "Name", "PricePerItem", "Type" },
                values: new object[,]
                {
                    { 1, "ConName1", 10f, "ConType1" },
                    { 2, "ConName2", 20f, "ConType2" },
                    { 3, "ConName3", 30f, "ConType3" }
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "IdClient", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, "CName1", "CSurame1" },
                    { 2, "CName2", "CSurame2" },
                    { 3, "CName3", "CSurame3" }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "IdEmployee", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, "EName1", "ESurame1" },
                    { 2, "EName2", "ESurame1" },
                    { 3, "EName3", "ESurame1" }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "IdOrder", "DateAccepted", "DateFinished", "IdClient", "IdEmployee", "Notes" },
                values: new object[] { 1, new DateTime(2020, 5, 11, 2, 55, 38, 371, DateTimeKind.Local).AddTicks(2574), new DateTime(2020, 6, 10, 2, 55, 38, 373, DateTimeKind.Local).AddTicks(8237), 1, 1, "OrderNotes1" });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "IdOrder", "DateAccepted", "DateFinished", "IdClient", "IdEmployee", "Notes" },
                values: new object[] { 2, new DateTime(2020, 4, 11, 2, 55, 38, 373, DateTimeKind.Local).AddTicks(9973), new DateTime(2020, 6, 9, 2, 55, 38, 373, DateTimeKind.Local).AddTicks(9996), 2, 2, "OrderNotes2" });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "IdOrder", "DateAccepted", "DateFinished", "IdClient", "IdEmployee", "Notes" },
                values: new object[] { 3, new DateTime(2020, 3, 11, 2, 55, 38, 374, DateTimeKind.Local).AddTicks(27), new DateTime(2020, 6, 8, 2, 55, 38, 374, DateTimeKind.Local).AddTicks(31), 3, 3, "OrderNotes3" });

            migrationBuilder.InsertData(
                table: "Confectionery_Order",
                columns: new[] { "IdConfectionery", "IdOrder", "Notes", "Quantity" },
                values: new object[] { 1, 1, "ConOrderNotes1", 1 });

            migrationBuilder.InsertData(
                table: "Confectionery_Order",
                columns: new[] { "IdConfectionery", "IdOrder", "Notes", "Quantity" },
                values: new object[] { 2, 2, "ConOrderNotes2", 2 });

            migrationBuilder.InsertData(
                table: "Confectionery_Order",
                columns: new[] { "IdConfectionery", "IdOrder", "Notes", "Quantity" },
                values: new object[] { 3, 3, "ConOrderNotes3", 3 });

            migrationBuilder.CreateIndex(
                name: "IX_Confectionery_Order_IdOrder",
                table: "Confectionery_Order",
                column: "IdOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdClient",
                table: "Order",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdEmployee",
                table: "Order",
                column: "IdEmployee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Confectionery_Order");

            migrationBuilder.DropTable(
                name: "Confectionery");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
