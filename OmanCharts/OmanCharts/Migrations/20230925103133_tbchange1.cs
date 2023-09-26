using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmanCharts.Migrations
{
    /// <inheritdoc />
    public partial class tbchange1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Statistics",
                table: "Statistics");

            migrationBuilder.RenameTable(
                name: "Statistics",
                newName: "tblStatistics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblStatistics",
                table: "tblStatistics",
                column: "StatisticId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblStatistics",
                table: "tblStatistics");

            migrationBuilder.RenameTable(
                name: "tblStatistics",
                newName: "Statistics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statistics",
                table: "Statistics",
                column: "StatisticId");
        }
    }
}
