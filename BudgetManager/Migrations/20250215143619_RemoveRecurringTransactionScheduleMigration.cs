using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetManager.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRecurringTransactionScheduleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecurringTransactionSchedules");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "RecurringTransactions",
                newName: "Interval");

            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "RecurringTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxOccurrences",
                table: "RecurringTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MonthlyDay",
                table: "RecurringTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeeklyDays",
                table: "RecurringTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearlyDay",
                table: "RecurringTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearlyMonth",
                table: "RecurringTransactions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "RecurringTransactions");

            migrationBuilder.DropColumn(
                name: "MaxOccurrences",
                table: "RecurringTransactions");

            migrationBuilder.DropColumn(
                name: "MonthlyDay",
                table: "RecurringTransactions");

            migrationBuilder.DropColumn(
                name: "WeeklyDays",
                table: "RecurringTransactions");

            migrationBuilder.DropColumn(
                name: "YearlyDay",
                table: "RecurringTransactions");

            migrationBuilder.DropColumn(
                name: "YearlyMonth",
                table: "RecurringTransactions");

            migrationBuilder.RenameColumn(
                name: "Interval",
                table: "RecurringTransactions",
                newName: "ScheduleId");

            migrationBuilder.CreateTable(
                name: "RecurringTransactionSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecurringTransactionId = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    MaxOccurrences = table.Column<int>(type: "int", nullable: true),
                    MonthlyDay = table.Column<int>(type: "int", nullable: true),
                    WeeklyDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearlyDay = table.Column<int>(type: "int", nullable: true),
                    YearlyMonth = table.Column<int>(type: "int", nullable: true)
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
    }
}
