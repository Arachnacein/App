namespace UI.Components;

public partial class StatisticsComponent
{
    [Inject] private IStringLocalizer<StatisticsComponent> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    private CategoriesDistributionModel CategoriesDistribution { get; set; }
    private List<MonthlyCategoriesDistribution> MonthlyCategoriesDistributionList { get; set; }
    private List<ChartSeries<double>> _series = new List<ChartSeries<double>>();
    private string[] _xaxisLabels = { };
    private const int MonthsWindowSize = 20;
    private static readonly string[] _categoryChartPalette = { "#2979FF", "#C0504D", "#FFC400" };
    private StackedBarChartOptions BarChartOptions { get; } = new()
    {
        XAxisLabelRotation = 90,
        YAxisFormat = "F2",
        Justify = Justify.Center,
        YAxisSuggestedMax = 100,
        FixedBarWidth = 20,
        ChartPalette = _categoryChartPalette
    };
    private ChartOptions PieChartOptions { get; } = new()
    {
        ChartPalette = _categoryChartPalette
    };
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
    private int WindowStartIndex { get; set; }
    private int MaxWindowStartIndex => Math.Max(0, (MonthlyCategoriesDistributionList?.Count ?? 0) - MonthsWindowSize);
    private bool ShowMonthsNavigator =>  SelectedYear == null && (MonthlyCategoriesDistributionList?.Count ?? 0) > MonthsWindowSize;
    private int? MinNavigatorYear => MonthlyCategoriesDistributionList?.FirstOrDefault()?.Year;
    private int? MaxNavigatorYear => MonthlyCategoriesDistributionList?.LastOrDefault()?.Year;

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
            var result = await HttpClient.GetFromJsonAsync<List<int>>
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

            var result = await HttpClient.GetFromJsonAsync<FilteredStatisticsModel>(url);

            if (result != null)
            {
                TotalSaves = result.TotalSaves;
                TotalExpenses = result.TotalExpenses;
                AverageSaves = result.AverageSaves;
                AverageExpenses = result.AverageExpenses;
                SavingsRate = result.SavingsRate;

                if (result.TransactionCount != null)
                    TransactionCountByCategory = result.TransactionCount;
            }
        }
        catch { }
        StateHasChanged();
    }

    private void OnMonthsWindowChanged(int newStartIndex)
    {
        WindowStartIndex = newStartIndex;
        UpdateChartSeries();
    }

    private void MoveWindowBackward()
    {
        if (WindowStartIndex <= 0)
            return;

        WindowStartIndex--;
        UpdateChartSeries();
    }

    private void MoveWindowForward()
    {
        if (WindowStartIndex >= MaxWindowStartIndex)
            return;

        WindowStartIndex++;
        UpdateChartSeries();
    }

    private async Task OnYearChanged(int? year)
    {
        SelectedYear = year;
        await GetFilteredStatistics(year);
        await GetMonthlyCategoriesDistribution(year);
    }

    private async Task GetThreeMonthsSaves()
    {
        Total3MonthsSaves = await HttpClient.GetFromJsonAsync<double>
            ($"/api/statistics/GetThreeMonthsSaves?userId={UserSessionService.UserId}");

        StateHasChanged();
    }

    private async Task GetThreeMonthsExpenses()
    {
        Total3MonthsExpenses = await HttpClient.GetFromJsonAsync<double>
            ($"/api/statistics/GetThreeMonthsExpenses?userId={UserSessionService.UserId}");

        StateHasChanged();
    }

    private async Task GetCategoriesDistribution()
    {
        CategoriesDistribution = await HttpClient.GetFromJsonAsync<CategoriesDistributionModel>
            ($"/api/statistics/GetCategoriesDistribution?userId={UserSessionService.UserId}");

        StateHasChanged();
    }

    private async Task GetMonthlyCategoriesDistribution(int? year = null)
    {
        var url = $"/api/statistics/GetMonthlyCategoriesDistribution?userId={UserSessionService.UserId}";

        if (year.HasValue)
            url += $"&year={year.Value}";

        MonthlyCategoriesDistributionList = await HttpClient.GetFromJsonAsync<List<MonthlyCategoriesDistribution>>(url);

        WindowStartIndex = MaxWindowStartIndex;
        UpdateChartSeries();
    }

    private void UpdateChartSeries()
    {
        var displayedMonths = SelectedYear.HasValue
            ? MonthlyCategoriesDistributionList
            : MonthlyCategoriesDistributionList.Skip(WindowStartIndex).Take(MonthsWindowSize).ToList();

        _xaxisLabels = displayedMonths
                        .Select(x => $"{x.Month:D2}'{x.Year % 100:D2}")
                        .ToArray();

        _series = new List<ChartSeries<double>>
        {
            new ChartSeries<double>{
                Name = Localizer["Saves"],
                Data = displayedMonths
                            .Select(item => Math.Round(item.Saves, 2)).ToArray()
            },
            new ChartSeries<double>{
                Name = Localizer["Fees"],
                Data = displayedMonths
                            .Select(item => Math.Round(item.Fees, 2)).ToArray()
            },
            new ChartSeries<double>{
                Name = Localizer["Entertainment"],
                Data = displayedMonths
                            .Select(item =>
                            {
                                var saves = Math.Round(item.Saves, 2);
                                var fees  = Math.Round(item.Fees, 2);
                                return Math.Round(100.0 - saves - fees, 2);
                            }).ToArray()
            }
        };
        StateHasChanged();
    }
}