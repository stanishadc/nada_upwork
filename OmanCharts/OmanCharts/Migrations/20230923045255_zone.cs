using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmanCharts.Migrations
{
    /// <inheritdoc />
    public partial class zone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zone",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "Zone",
                table: "AspNetUsers",
                newName: "ZoneId");

            migrationBuilder.AddColumn<Guid>(
                name: "ZoneId",
                table: "Statistics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ZoneId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    ZoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZoneName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.ZoneId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ZoneId",
                table: "AspNetUsers",
                newName: "Zone");

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "Statistics",
                type: "varchar(25)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "Projects",
                type: "varchar(25)",
                nullable: false,
                defaultValue: "");
        }
    }
}
