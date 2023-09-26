using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmanCharts.Migrations
{
    /// <inheritdoc />
    public partial class pc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectCategory",
                table: "tblStatistics",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(4)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectCategory",
                table: "tblStatistics",
                type: "varchar(4)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)");
        }
    }
}
