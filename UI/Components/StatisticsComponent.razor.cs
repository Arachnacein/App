
using Microsoft.AspNetCore.Components;

namespace UI.Components
{
    public partial class StatisticsComponent
    {
        [Inject] HttpClient httpClient { get; set; }
        private double TotalExpensesProp { get; set; }
        private double TotalSavesProp { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetTotalExpenses();
            await GetTotalSaves();
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
    }
}