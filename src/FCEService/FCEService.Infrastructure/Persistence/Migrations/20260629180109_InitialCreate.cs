using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCEService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculatedMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BMR = table.Column<double>(type: "float", nullable: false),
                    TDEE = table.Column<double>(type: "float", nullable: false),
                    CalorieTarget = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FitnessPlanConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Goal = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CalorieMin = table.Column<double>(type: "float", nullable: false),
                    CalorieMax = table.Column<double>(type: "float", nullable: false),
                    ExternalPlanId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WorkoutsPerWeek = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessPlanConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAssignedPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ExternalPlanId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssignedPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFitnessStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Goal = table.Column<int>(type: "int", nullable: false),
                    ActivityLevel = table.Column<int>(type: "int", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFitnessStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPlanHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExternalPlanId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReasonForChange = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlanHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanHistory_UserId",
                table: "UserPlanHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculatedMetrics");

            migrationBuilder.DropTable(
                name: "FitnessPlanConfigs");

            migrationBuilder.DropTable(
                name: "UserAssignedPlans");

            migrationBuilder.DropTable(
                name: "UserFitnessStats");

            migrationBuilder.DropTable(
                name: "UserPlanHistory");
        }
    }
}
