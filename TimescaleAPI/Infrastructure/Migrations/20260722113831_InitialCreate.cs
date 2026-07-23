using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimescaleAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Origins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateDelta = table.Column<int>(type: "integer", nullable: false),
                    DateMin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExecutionTimeAverage = table.Column<double>(type: "double precision", nullable: false),
                    ValueAverage = table.Column<double>(type: "double precision", nullable: false),
                    ValueMedian = table.Column<double>(type: "double precision", nullable: false),
                    ValueMax = table.Column<double>(type: "double precision", nullable: false),
                    ValueMin = table.Column<double>(type: "double precision", nullable: false),
                    OriginId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Values",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExecutionTime = table.Column<int>(type: "integer", nullable: false),
                    IndicatorValue = table.Column<double>(type: "double precision", nullable: false),
                    OriginId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Values_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_OriginId",
                table: "Results",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_OriginId",
                table: "Values",
                column: "OriginId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Values");

            migrationBuilder.DropTable(
                name: "Origins");
        }
    }
}
