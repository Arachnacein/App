
using Microsoft.AspNetCore.Components;

namespace UI.Components
{
    public partial class StatisticsComponent
    {
        [Inject] HttpClient httpClient { get; set; }
        private double TotalExpensesProp { get; set; }
        private double TotalSavesProp { get; set; }
        private double Total3MonthsExpensesProp { get; set; }
        private double Total3MonthsSavesProp { get; set; }
        private double AverageExpensesProp { get; set; }
        private double AverageSavesProp { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetTotalExpenses();
            await GetTotalSaves();
            await GetAverageExpenses();
            await GetAverageSaves();
            await GetThreeMonthsSaves();
            await GetThreeMonthsExpenses();
        }
        private async Task GetTotalExpenses()
        {
            TotalExpensesProp = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetTotalExpenses");
            StateHasChanged();
        }
        private async Task GetTotalSaves()
        {
            TotalSavesProp = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetTotalSaves");
            StateHasChanged();
        }
        private async Task GetAverageExpenses()
        {
            AverageExpensesProp = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetAverageExpenses");
            StateHasChanged();
        }
        private async Task GetAverageSaves()
        {
            AverageSavesProp = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetAverageSaves");
            StateHasChanged();
        }        
        private async Task GetThreeMonthsSaves()
        {
            Total3MonthsSavesProp = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetThreeMonthsSaves");
            StateHasChanged();
        }        
        private async Task GetThreeMonthsExpenses()
        {
            Total3MonthsExpensesProp = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetThreeMonthsExpenses");
            StateHasChanged();
        }
    }
}