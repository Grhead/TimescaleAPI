using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimescaleAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameNameHashColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameHash",
                table: "Origins",
                newName: "FileName");

            migrationBuilder.RenameIndex(
                name: "IX_Origins_NameHash",
                table: "Origins",
                newName: "IX_Origins_FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Origins",
                newName: "NameHash");

            migrationBuilder.RenameIndex(
                name: "IX_Origins_FileName",
                table: "Origins",
                newName: "IX_Origins_NameHash");
        }
    }
}
