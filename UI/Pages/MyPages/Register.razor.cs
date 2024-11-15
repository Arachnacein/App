﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages
{
    public partial class Register
    {
        [Inject] private IStringLocalizer<Register> Localizer {  get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] private RegistrationViewModelValidator RegistrationModelValidator { get; set; }
        private RegistrationViewModel RegistrationModel = new RegistrationViewModel();
        private MudForm Form;

        private async Task Submit()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                // send data to api
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
                    Snackbar.Add(Localizer["RegisterFail"], Severity.Error);
            }
            else
                Snackbar.Add(Localizer["InvalidForm"], Severity.Warning);
        }
    }
}