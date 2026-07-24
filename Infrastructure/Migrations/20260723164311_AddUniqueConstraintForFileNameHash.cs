using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimescaleAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintForFileNameHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Origins_NameHash",
                table: "Origins",
                column: "NameHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Origins_NameHash",
                table: "Origins");
        }
    }
}
