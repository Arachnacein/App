namespace BudgetManager.Features.Statistics;

public class CategoriesModel
{
    public double Saves { get; set; }
    public double Fees { get; set; }
    public double Entertainment { get; set; }
}
public class MonthlyCategoriesModel : CategoriesModel
{
    public int Month { get; set; }
    public int Year { get; set; }
}
public class TransactionCountModel
{
    public int Saves { get; set; }
    public int Fees { get; set; }
    public int Entertainment { get; set; }
}
public class FilteredStatisticsModel
{
    public double TotalSaves { get; set; }
    public double TotalExpenses { get; set; }
    public double AverageSaves { get; set; }
    public double AverageExpenses { get; set; }
    public double SavingsRate { get; set; }
    public TransactionCountModel TransactionCount { get; set; }
}