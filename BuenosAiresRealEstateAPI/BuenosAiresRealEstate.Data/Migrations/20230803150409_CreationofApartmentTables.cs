using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BuenosAiresRealEstate.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreationofApartmentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApartmentComplexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplexName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentComplexes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentUnits",
                columns: table => new
                {
                    ApartmentUnitId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SquareMeters = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApartmentComplexId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentUnits", x => x.ApartmentUnitId);
                    table.ForeignKey(
                        name: "FK_ApartmentUnits_ApartmentComplexes_ApartmentComplexId",
                        column: x => x.ApartmentComplexId,
                        principalTable: "ApartmentComplexes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApartmentComplexes",
                columns: new[] { "Id", "Address", "Amenities", "ComplexName", "CreatedDate", "ImageUrl", "Owner", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Juncal 2736", "Pool, Playroom, Laundry Room", "Juncal Suites", new DateTime(2023, 8, 3, 12, 4, 9, 698, DateTimeKind.Local).AddTicks(1400), "Images\\Juncal.jpg", "JM Iriarte", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Guise 1784", "Playroom, Laundry Room", "Guise Suites", new DateTime(2023, 8, 3, 12, 4, 9, 698, DateTimeKind.Local).AddTicks(1412), "Images\\Guise.jpg", "JM Iriarte", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Beruti 3891", "Pool, Playroom", "Beruti Tower", new DateTime(2023, 8, 3, 12, 4, 9, 698, DateTimeKind.Local).AddTicks(1414), "Images\\Beruti.jpg", "MH Jaunarena", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Paraguay 1800", "Pool, Laundry Room", "Paraguay Complex", new DateTime(2023, 8, 3, 12, 4, 9, 698, DateTimeKind.Local).AddTicks(1416), "Images\\Paraguay.jpg", "MH Iriarte", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Alcorta 1900", "Pool", "Alcorta Palace", new DateTime(2023, 8, 3, 12, 4, 9, 698, DateTimeKind.Local).AddTicks(1418), "Images\\Alcorta.jpg", "OA Iriarte", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Santa Fe 2000", "Playroom", "Santa Fe Apartments", new DateTime(2023, 8, 3, 12, 4, 9, 698, DateTimeKind.Local).AddTicks(1420), "Images\\SantaFe.jpg", "HM Iriarte", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Mansilla 1000", "", "Mansilla Suites", new DateTime(2023, 8, 3, 12, 4, 9, 698, DateTimeKind.Local).AddTicks(1422), "Images\\Mansilla.jpg", "OA Iriarte", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentUnits_ApartmentComplexId",
                table: "ApartmentUnits",
                column: "ApartmentComplexId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentUnits");

            migrationBuilder.DropTable(
                name: "ApartmentComplexes");
        }
    }
}
