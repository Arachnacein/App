namespace UI.Extensions;

public static class CategoriesExtesions
{
    /// <summary>
    ///     Converts a CategoriesDistributionModel to a list of ChartSeries<double> for use in a pie chart. 
    ///     If the model is null, it returns a default series indicating invalid data.
    /// </summary>
    /// <param name="model">The categories distribution model.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <returns>The list of chart series.</returns>
    public static List<ChartSeries<double>> ToPieChartSeries(this CategoriesDistributionModel model, IStringLocalizer localizer)
    {
        if (model != null)
            return new List<ChartSeries<double>>
            {
                new() { Data = new double[] { model.Saves, model.Fees, model.Entertainment } }
            };
        else
            return new List<ChartSeries<double>>
            {
                new() { Name = localizer["InvalidData"], Data = new double[] { 100.0 } }
            };
    }

    /// <summary>
    ///    Generates labels for a pie chart based on the categories distribution model. 
    ///    Each label includes the category name and its corresponding percentage value.
    /// </summary>
    /// <param name="model">The categories distribution model.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <returns>The array of pie chart labels.</returns>
    public static string[] GetPieChartLabels(this CategoriesDistributionModel model, IStringLocalizer localizer)
    {
        if (model != null)
            return new[]
            {
                $"{localizer["Saves"]} {Math.Round(model.Saves, 2)}%",
                $"{localizer["Fees"]} {Math.Round(model.Fees, 2)}%",
                $"{localizer["Entertainment"]} {Math.Round(model.Entertainment, 2)}%",
            };
        else
            return new[] { (string)localizer["InvalidData"] };
    }
}