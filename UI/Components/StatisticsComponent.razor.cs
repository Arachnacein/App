using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Globalization;
using UI.Models;

namespace UI.Components;

public partial class StatisticsComponent
{
    [Inject] private IStringLocalizer<StatisticsComponent> Localizer { get; set; }
    [Inject] private HttpClient httpClient { get; set; }
    private CategoriesDistributionModel CategoriesDistribution { get; set; }
    private List<MonthlyCategoriesDistribution> MonthlyCategoriesDistributionList { get; set; }
    private List<ChartSeries> Series = new List<ChartSeries>();
    private string[] XaxisLabels = { };
    private double TotalExpenses { get; set; }
    private double TotalSaves { get; set; }
    private double Total3MonthsExpenses { get; set; }
    private double Total3MonthsSaves { get; set; }
    private double AverageExpenses { get; set; }
    private double AverageSaves { get; set; }
    private double SavingsRate { get; set; }
    private TransactionCountModel TransactionCountByCategory { get; set; } = new();
    private List<int> AvailableYears { get; set; } = new();
    private int? SelectedYear { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (UserSessionService != null && UserSessionService.UserId != Guid.Empty)
        {
            await GetAvailableYears();
            await GetFilteredStatistics(null);
            await GetThreeMonthsSaves();
            await GetThreeMonthsExpenses();
            await GetCategoriesDistribution();
            await GetMonthlyCategoriesDistribution();
        }
    }

    private async Task GetAvailableYears()
    {
        try
        {
            var result = await httpClient
                .GetFromJsonAsync<List<int>>
                ($"/api/statistics/GetAvailableYears?userId={UserSessionService.UserId}");
            if (result != null)
                AvailableYears = result;
        }
        catch { }
        StateHasChanged();
    }

    private async Task GetFilteredStatistics(int? year)
    {
        try
        {
            var url = $"/api/statistics/GetFilteredStatistics?userId={UserSessionService.UserId}";
            if (year.HasValue)
                url += $"&year={year.Value}";

            var result = await httpClient
                .GetFromJsonAsync<FilteredStatisticsModel>(url);

            if (result != null)
            {
                TotalSaves      = result.TotalSaves;
                TotalExpenses   = result.TotalExpenses;
                AverageSaves    = result.AverageSaves;
                AverageExpenses = result.AverageExpenses;
                SavingsRate     = result.SavingsRate;
                if (result.TransactionCount != null)
                    TransactionCountByCategory = result.TransactionCount;
            }
        }
        catch { }
        StateHasChanged();
    }

    private async Task OnYearChanged(int? year)
    {
        SelectedYear = year;
        await GetFilteredStatistics(year);
    }

    private async Task GetThreeMonthsSaves()
    {
        Total3MonthsSaves = await httpClient
            .GetFromJsonAsync<double>
            ($"/api/statistics/GetThreeMonthsSaves?userId={UserSessionService.UserId}");
        StateHasChanged();
    }

    private async Task GetThreeMonthsExpenses()
    {
        Total3MonthsExpenses = await httpClient
            .GetFromJsonAsync<double>
            ($"/api/statistics/GetThreeMonthsExpenses?userId={UserSessionService.UserId}");
        StateHasChanged();
    }

    private async Task GetCategoriesDistribution()
    {
        CategoriesDistribution = await httpClient
            .GetFromJsonAsync<CategoriesDistributionModel>
            ($"/api/statistics/GetCategoriesDistribution?userId={UserSessionService.UserId}");
        StateHasChanged();
    }

    private async Task GetMonthlyCategoriesDistribution()
    {
        MonthlyCategoriesDistributionList = await httpClient
            .GetFromJsonAsync<List<MonthlyCategoriesDistribution>>
            ($"/api/statistics/GetMonthlyCategoriesDistribution?userId={UserSessionService.UserId}");

        XaxisLabels = MonthlyCategoriesDistributionList
                        .Select(x => new DateTime(x.Year, x.Month, 1)
                            .ToString("MM-yyyy", CultureInfo.CurrentCulture))
                        .ToArray();

        Series = new List<ChartSeries>
        {
            new ChartSeries{
                Name = Localizer["Saves"],
                Data = MonthlyCategoriesDistributionList
                            .Select(item => item.Saves).ToArray()
            },
            new ChartSeries{
                Name = Localizer["Fees"],
                Data = MonthlyCategoriesDistributionList
                            .Select(item => item.Fees).ToArray()
            },
            new ChartSeries{
                Name = Localizer["Entertainment"],
                Data = MonthlyCategoriesDistributionList
                            .Select(item => item.Entertainment).ToArray()
            }
        };
        StateHasChanged();
    }
}
