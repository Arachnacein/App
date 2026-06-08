using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models;

namespace UI.Extensions;

public static class CategoriesExtesions
{
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