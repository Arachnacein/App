namespace UI.Models
{
    public class FilteredStatisticsModel
    {
        public double TotalSaves { get; set; }
        public double TotalExpenses { get; set; }
        public double AverageSaves { get; set; }
        public double AverageExpenses { get; set; }
        public double SavingsRate { get; set; }
        public TransactionCountModel TransactionCount { get; set; } = new();
    }
}
