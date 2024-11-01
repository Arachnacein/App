namespace BudgetManager.Features.Statistics
{
    public class CategoriesModel
    {
        public double Saves { get; set; }
        public double Fees { get; set; }
        public double Enterntainment { get; set; }
    }
    public class MonthlyCategoriesModel : CategoriesModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}