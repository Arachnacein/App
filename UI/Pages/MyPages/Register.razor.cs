using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages
{
    public partial class Register
    {
        [Inject] private IStringLocalizer<Register> Localizer {  get; set; }
        [Inject] private RegistrationViewModelValidator RegistrationModelValidator { get; set; }
        private RegistrationViewModel RegistrationModel = new RegistrationViewModel();
        private MudForm Form;

        private async Task Submit()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                // send data to api



                Snackbar.Add(Localizer["RegisterSuccess"], Severity.Success);
                Navigation.NavigateTo("/", false);
            }
            else
            {
                Snackbar.Add(Localizer["InvalidForm"], Severity.Warning);
            }
        }
    }
}