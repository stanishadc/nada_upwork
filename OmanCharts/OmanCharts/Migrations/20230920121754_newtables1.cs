using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmanCharts.Migrations
{
    /// <inheritdoc />
    public partial class newtables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectNumber = table.Column<string>(type: "varchar(25)", nullable: false),
                    ProjectName = table.Column<string>(type: "varchar(25)", nullable: false),
                    Zone = table.Column<string>(type: "varchar(25)", nullable: false),
                    ProjectCategory = table.Column<string>(type: "varchar(25)", nullable: false),
                    Year = table.Column<string>(type: "varchar(25)", nullable: false),
                    FinanceEntity = table.Column<string>(type: "varchar(25)", nullable: false),
                    Contractor = table.Column<string>(type: "varchar(25)", nullable: false),
                    Consultant = table.Column<string>(type: "varchar(25)", nullable: false),
                    Progress = table.Column<string>(type: "varchar(25)", nullable: false),
                    ProjectPeriod = table.Column<string>(type: "varchar(25)", nullable: false),
                    StartDateContract = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateContract = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    timeExtension = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: true),
                    IdentifiedCost = table.Column<double>(type: "float", nullable: true),
                    Expenses = table.Column<double>(type: "float", nullable: true),
                    RemainingCost = table.Column<double>(type: "float", nullable: true),
                    ChangeRequestCost = table.Column<double>(type: "float", nullable: true),
                    RequiredAction = table.Column<string>(type: "varchar(45)", nullable: false),
                    Challenges = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zone = table.Column<string>(type: "varchar(25)", nullable: false),
                    OmanizationRate = table.Column<double>(type: "float", nullable: true),
                    TotalLabour = table.Column<double>(type: "float", nullable: true),
                    Investments = table.Column<double>(type: "float", nullable: true),
                    TotalInvestors = table.Column<double>(type: "float", nullable: true),
                    TotalProjects = table.Column<double>(type: "float", nullable: true),
                    Year = table.Column<string>(type: "varchar(4)", nullable: false),
                    ProjectCategory = table.Column<string>(type: "varchar(4)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Statistics");
        }
    }
}
