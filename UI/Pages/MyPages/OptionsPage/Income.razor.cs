using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Models;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class Income
    {
        [Inject] HttpClient httpClient {  get; set; }
        [Inject] IDialogService dialogService { get; set; }
        private List<IncomeViewModel> incomes = new List<IncomeViewModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadIncomes();
        }

        private async Task LoadIncomes()
        {
            incomes = await httpClient.GetFromJsonAsync<List<IncomeViewModel>>("/api/income");
            StateHasChanged();
        }
        private async Task EditIncome(IncomeViewModel model)
        {
            DialogOptions options = new DialogOptions{ CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };
            var parameters = new DialogParameters();
            parameters[nameof(model)] = model;
            parameters["Refresh"] = new Func<Task>(LoadIncomes);

            await dialogService.ShowAsync<EditIncomeDialog>("Edit", parameters, options);
        }
    }
}