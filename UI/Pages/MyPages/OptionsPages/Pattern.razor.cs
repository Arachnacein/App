using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPages;

public partial class Pattern
{
    [Inject] private ISnackbar snackbar { get; set; }
    [Inject] private IDialogService dialogService { get; set; }
    [Inject] private HttpClient httpClient { get; set; }
    [Inject] private PatternViewModelValidator PatternValidator { get; set; }
    private MudForm Form;
    private PatternViewModel Model = new PatternViewModel();
    private List<MonthPatternViewModel> patterns = new List<MonthPatternViewModel>();
    private List<PatternViewModel> patternsList = new List<PatternViewModel>();

    protected override async Task OnInitializedAsync()
    {
        await LoadMonthPatterns();
        await LoadPatterns();
    }

    private async Task LoadPatterns()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        patternsList = await httpClient.GetFromJsonAsync<List<PatternViewModel>>($"/api/pattern?userId={UserSessionService.UserId}");
        StateHasChanged();
    }

    private async Task AddPattern()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        await Form.Validate();
        if (!Form.IsValid)
            return;

        if (Model.Value_Saves + Model.Value_Fees + Model.Value_Entertainment != 100)
        {
            snackbar.Add(Localizer["FailAddSnackbarInvalidValues"], Severity.Warning);
            return;
        }

        Model.UserId = UserSessionService.UserId;
        var request = await httpClient.PostAsJsonAsync<PatternViewModel>($"/api/pattern?userId={UserSessionService.UserId}", Model);
        if (request.StatusCode == System.Net.HttpStatusCode.Created)
        {
            snackbar.Add(Localizer["SuccessAddSnackbar"], Severity.Success);
            await LoadPatterns();
        }
        else if (request.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
            snackbar.Add(Localizer["FailAddSnackbarDuplicate"], Severity.Warning);
        else if (request.StatusCode == System.Net.HttpStatusCode.Conflict)
            snackbar.Add(Localizer["FailAddSnackbarInvalidValues"], Severity.Warning);
        else
            snackbar.Add(Localizer["FailAddSnackbar"], Severity.Error);
    }

    private async Task DeletePattern(PatternViewModel pattern)
    {
        var confirmed = await dialogService.ShowMessageBoxAsync(
            Localizer["DeleteConfirmTitle"],
            $"{pattern.Name}?",
            yesText: Localizer["DeleteConfirmYes"],
            cancelText: Localizer["DeleteConfirmCancel"]);

        if (confirmed != true)
            return;

        var request = await httpClient.DeleteAsync($"/api/pattern/{pattern.Id}/user/{UserSessionService.UserId}");
        if (request.IsSuccessStatusCode)
        {
            snackbar.Add(Localizer["SuccessDeleteSnackbar"], Severity.Success);
            await LoadPatterns();
        }
        else if (request.StatusCode == System.Net.HttpStatusCode.Conflict)
            snackbar.Add(Localizer["FailDeleteSnackbarInUse"], Severity.Warning);
        else
            snackbar.Add(Localizer["FailDeleteSnackbar"], Severity.Error);
    }

    private async Task LoadMonthPatterns()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        patterns = await httpClient.GetFromJsonAsync<List<MonthPatternViewModel>>($"/api/monthpattern/GetAllWithPattern?userId={UserSessionService.UserId}");
        StateHasChanged();
    }

    private async Task EditMonthPattern(MonthPatternViewModel contextModel)
    {
        var parameters = new DialogParameters();
        parameters[nameof(contextModel)] = contextModel;
        parameters["Refresh"] = new Func<Task>(LoadMonthPatterns);
        var options = new DialogOptions { CloseOnEscapeKey = true };

        await dialogService.ShowAsync<EditMonthPatternDialog>(Localizer["EditPatternDialogHeader", contextModel.Date.Month, contextModel.Date.Year], parameters, options);
    }
}