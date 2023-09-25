using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmanCharts.Migrations
{
    /// <inheritdoc />
    public partial class tbchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Statistics",
                newName: "StatisticId");

            migrationBuilder.RenameColumn(
                name: "timeExtension",
                table: "Projects",
                newName: "TimeExtension");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Projects",
                newName: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatisticId",
                table: "Statistics",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TimeExtension",
                table: "Projects",
                newName: "timeExtension");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Projects",
                newName: "Id");
        }
    }
}
