using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages
{
    public partial class Register
    {
        [Inject] private IStringLocalizer<Register> Localizer {  get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] private RegistrationViewModelValidator RegistrationModelValidator { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        private RegistrationViewModel RegistrationModel = new RegistrationViewModel();
        private MudForm Form;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        InputType PasswordInput = InputType.Password;
        bool isShow;

        private async Task Submit()
        {
            await Form.Validate();
            
            var recaptchaResponse = await JSRuntime.InvokeAsync<string>("grecaptcha.getResponse");
            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                Snackbar.Add("Please complete the CAPTCHA", Severity.Error);
                return;
            }

            var isValidCaptcha = await ValidateCaptcha(recaptchaResponse);
            if (!isValidCaptcha)
            {
                Snackbar.Add("Invalid CAPTCHA", Severity.Error);
                return;
            }

            if (Form.IsValid)
            {
                var requestBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("firstname", RegistrationModel.FirstName),
                    new KeyValuePair<string, string>("lastname", RegistrationModel.LastName),
                    new KeyValuePair<string, string>("username", RegistrationModel.Username),
                    new KeyValuePair<string, string>("email", RegistrationModel.Email),
                    new KeyValuePair<string, string>("enabled", "true"),
                    new KeyValuePair<string, string>("password", RegistrationModel.Password),
                });


                var response = await httpClient.PostAsync("/api/register", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    Snackbar.Add(Localizer["RegisterSuccess"], Severity.Success);
                    Navigation.NavigateTo("/", false);
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
                    Snackbar.Add(Localizer[$"Error_{errorResponse.ErrorCode}"], Severity.Warning);
                }
            }
            else
                Snackbar.Add(Localizer["InvalidForm"], Severity.Warning);
        }
        private async Task<bool> ValidateCaptcha(string recaptchaResponse)
        {
            var secretKey = "6LfCwL8qAAAAAMMPjvFljaGn8iMO7Jb6zSWN9Go3";
            var response = await httpClient.PostAsJsonAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}", new { });
            var captchaResult = await response.Content.ReadFromJsonAsync<CaptchaResult>();
            return captchaResult.Success;
        }
        private void ShowPassword()
        {
            if (isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }
    }
}