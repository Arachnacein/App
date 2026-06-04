using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Reflection;
using UI.Models;

namespace UI.Extensions;

public static class CategoriesExtesions
{
    public static List<ChartSeries<double>> ToPieChartSeries(this CategoriesDistributionModel model, IStringLocalizer localizer)
    {
        if (model != null)
            return new List<ChartSeries<double>>
            {
                new() { Name = $"{localizer["Saves"]} {Math.Round(model.Saves, 2)}%",         Data = new[] { model.Saves } },
                new() { Name = $"{localizer["Fees"]} {Math.Round(model.Fees, 2)}%",           Data = new[] { model.Fees } },
                new() { Name = $"{localizer["Entertainment"]} {Math.Round(model.Entertainment, 2)}%", Data = new[] { model.Entertainment } },
            };
        else
            return new List<ChartSeries<double>> { new() { Name = localizer["InvalidData"], Data = new[] { 100.0 } } };
    }

    public static string[] GetPropertyNames(this CategoriesDistributionModel model, IStringLocalizer localizer)
    {
        if (model != null)
        {
            var tab = model.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(prop => localizer[prop.Name] + " ")
                .ToArray();

            tab[0] +=  Math.Round(model.Saves, 2).ToString() + "%";
            tab[1] += Math.Round(model.Fees, 2).ToString() + "%";
            tab[2] += Math.Round(model.Entertainment, 2).ToString() + "%";

            return tab;
        }
        else return new string[] { localizer["InvalidData"] };
    }
}