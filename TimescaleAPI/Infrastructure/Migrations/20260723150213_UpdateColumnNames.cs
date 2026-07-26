using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimescaleAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValueMin",
                table: "Results",
                newName: "MinValue");

            migrationBuilder.RenameColumn(
                name: "ValueMedian",
                table: "Results",
                newName: "MedianValue");

            migrationBuilder.RenameColumn(
                name: "ValueMax",
                table: "Results",
                newName: "MaxValue");

            migrationBuilder.RenameColumn(
                name: "ValueAverage",
                table: "Results",
                newName: "AvgValue");

            migrationBuilder.RenameColumn(
                name: "ExecutionTimeAverage",
                table: "Results",
                newName: "AvgExecutionTime");

            migrationBuilder.RenameColumn(
                name: "DateMin",
                table: "Results",
                newName: "MinDate");

            migrationBuilder.RenameColumn(
                name: "DateDelta",
                table: "Results",
                newName: "DeltaDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinValue",
                table: "Results",
                newName: "ValueMin");

            migrationBuilder.RenameColumn(
                name: "MinDate",
                table: "Results",
                newName: "DateMin");

            migrationBuilder.RenameColumn(
                name: "MedianValue",
                table: "Results",
                newName: "ValueMedian");

            migrationBuilder.RenameColumn(
                name: "MaxValue",
                table: "Results",
                newName: "ValueMax");

            migrationBuilder.RenameColumn(
                name: "DeltaDate",
                table: "Results",
                newName: "DateDelta");

            migrationBuilder.RenameColumn(
                name: "AvgValue",
                table: "Results",
                newName: "ValueAverage");

            migrationBuilder.RenameColumn(
                name: "AvgExecutionTime",
                table: "Results",
                newName: "ExecutionTimeAverage");
        }
    }
}
