﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;

namespace UI.Components.Dialogs
{
    public partial class DeleteTransactionDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Parameter] public TransactionViewModel model { get; set; }
        private TransactionViewModel DialogModel = new TransactionViewModel();

        protected override Task OnInitializedAsync()
        {
            DialogModel = model;
            return base.OnInitializedAsync();
        }
        private async Task Submit()
        {
            var request = await httpClient.DeleteAsync($"/api/budget/{DialogModel.Id}");
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add("Transaction deleted successfully.", Severity.Success);
                MudDialog.Cancel();
            }
            else
                snackbar.Add("Something went wrong", Severity.Error);
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}
