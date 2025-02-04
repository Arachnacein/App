using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetManager.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurringTransactionsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecurringTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurringTransactionSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    WeeklyDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthlyDay = table.Column<int>(type: "int", nullable: true),
                    YearlyMonth = table.Column<int>(type: "int", nullable: true),
                    YearlyDay = table.Column<int>(type: "int", nullable: true),
                    MaxOccurrences = table.Column<int>(type: "int", nullable: true),
                    RecurringTransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringTransactionSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringTransactionSchedules_RecurringTransactions_RecurringTransactionId",
                        column: x => x.RecurringTransactionId,
                        principalTable: "RecurringTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransactionSchedules_RecurringTransactionId",
                table: "RecurringTransactionSchedules",
                column: "RecurringTransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecurringTransactionSchedules");

            migrationBuilder.DropTable(
                name: "RecurringTransactions");
        }
    }
}
