
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Runtime.CompilerServices;
using UI.Models;

namespace UI.Components
{
    public partial class StatisticsComponent
    {
        [Inject] HttpClient httpClient { get; set; }
        private double TotalExpenses { get; set; }
        private double TotalSaves { get; set; }
        private double Total3MonthsExpenses { get; set; }
        private double Total3MonthsSaves { get; set; }
        private double AverageExpenses { get; set; }
        private double AverageSaves { get; set; }
        private CategoriesDistributionModel CategoriesDistribution { get; set; }
        private List<MonthlyCategoriesDistribution> MonthlyCategoriesDistributionList { get; set; }
        private List<ChartSeries> Series = new List<ChartSeries>();
        string[] XaxisLabels = { };



        protected override async Task OnInitializedAsync()
        {
            await GetTotalExpenses();
            await GetTotalSaves();
            await GetAverageExpenses();
            await GetAverageSaves();
            await GetThreeMonthsSaves();
            await GetThreeMonthsExpenses();
            await GetCategoriesDistribution();
            await GetMonthlyCategoriesDistribution();
        }
        private async Task GetTotalExpenses()
        {
            TotalExpenses = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetTotalExpenses");
            StateHasChanged();
        }
        private async Task GetTotalSaves()
        {
            TotalSaves = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetTotalSaves");
            StateHasChanged();
        }
        private async Task GetAverageExpenses()
        {
            AverageExpenses = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetAverageExpenses");
            StateHasChanged();
        }
        private async Task GetAverageSaves()
        {
            AverageSaves = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetAverageSaves");
            StateHasChanged();
        }        
        private async Task GetThreeMonthsSaves()
        {
            Total3MonthsSaves = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetThreeMonthsSaves");
            StateHasChanged();
        }        
        private async Task GetThreeMonthsExpenses()
        {
            Total3MonthsExpenses = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetThreeMonthsExpenses");
            StateHasChanged();
        }       
        
        private async Task GetCategoriesDistribution()
        {
            CategoriesDistribution = await httpClient.GetFromJsonAsync<CategoriesDistributionModel>("/api/statistics/GetCategoriesDistribution");
            StateHasChanged();
        }       
        
        private async Task GetMonthlyCategoriesDistribution()
        {
            MonthlyCategoriesDistributionList = await httpClient.GetFromJsonAsync<List<MonthlyCategoriesDistribution>>("/api/statistics/GetMonthlyCategoriesDistribution");

            XaxisLabels = MonthlyCategoriesDistributionList
                                .Select(x => new DateTime(x.Year, x.Month, 1)
                                    .ToString("MM-yyyy"))
                                .ToArray();

            Series = new List<ChartSeries>
            {
                new ChartSeries{
                    Name = "Saves",
                    Data = MonthlyCategoriesDistributionList
                                .Select(item => item.Saves).ToArray()
                },
                new ChartSeries{
                    Name = "Fees",
                    Data = MonthlyCategoriesDistributionList
                                .Select(item => item.Fees).ToArray()
                },
                new ChartSeries{
                    Name = "Entertainment",
                    Data = MonthlyCategoriesDistributionList
                                .Select(item => item.Enterntainment).ToArray()
                }
            };
            StateHasChanged();
        }
    }
}