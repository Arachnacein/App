
using Microsoft.AspNetCore.Components;

namespace UI.Components
{
    public partial class StatisticsComponent
    {
        [Inject] HttpClient httpClient { get; set; }
        private double TotalExpensesProp { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetTotalExpenses();
        }
        private async Task GetTotalExpenses()
        {
            TotalExpensesProp = await httpClient.GetFromJsonAsync<double>("/api/statistics/GetTotalExpenses");
            StateHasChanged();
        }
    }
}