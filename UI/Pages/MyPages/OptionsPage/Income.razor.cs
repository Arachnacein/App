using Microsoft.AspNetCore.Components;
using UI.Models;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class Income
    {
        [Inject] HttpClient httpClient {  get; set; }
        private List<IncomeViewModel> incomes = new List<IncomeViewModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadIncomes();
        }

        private async Task LoadIncomes()
        {
            incomes = await httpClient.GetFromJsonAsync<List<IncomeViewModel>>("/api/income");
        }
        private async Task EditIncome(IncomeViewModel model)
        {

        }
    }
}
