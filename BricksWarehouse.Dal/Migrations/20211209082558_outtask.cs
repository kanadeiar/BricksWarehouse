using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BricksWarehouse.Dal.Migrations
{
    public partial class outtask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    TruckNumber = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Loaded = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutTasks_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutTasks_ProductTypeId",
                table: "OutTasks",
                column: "ProductTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutTasks");
        }
    }
}
