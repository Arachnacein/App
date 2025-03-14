﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Models.ViewModels;
using UI.Services;

namespace UI.Pages.MyPages.OptionsPages
{
    public partial class Account
    {
        [Inject] private IStringLocalizer<Account> Localizer {  get; set; }
        [Inject] private IDialogService dialogService { get; set; }
        private UserDetailsViewModel UserDetails = new UserDetailsViewModel();

        protected override async Task OnInitializedAsync()
        {
            await LoadDataToUserDetailsModel();
        }

        private async Task LoadDataToUserDetailsModel()
        {
            UserDetails.UserId = UserSessionService.UserId;
            UserDetails.FirstName = UserSessionService.Name;
            UserDetails.LastName = UserSessionService.Surname;
            UserDetails.Username = UserSessionService.Username;
            UserDetails.Email = UserSessionService.Email;
            UserDetails.Roles = UserSessionService.Roles
                                    .Where(role => role.Contains("user") || role.Contains("admin"))
                                    .ToList() ?? new List<string>();
            UserDetails.AccountCreatedDate = UserSessionService.AccountCreatedDate;
            UserDetails.SessionExpiryDate = UserSessionService.TokenExpiryDate;
            UserDetails.EmailVerified = UserSessionService.EmailVerified;

        }
        private async Task EditData(string variableName)
        {
            switch (variableName)
            {
                case nameof(UserDetails.FirstName):
                    await OpenDialog(nameof(UserDetails.FirstName));
                    break;                
                case nameof(UserDetails.LastName):
                    await OpenDialog(nameof(UserDetails.LastName));
                    break;                
                case nameof(UserDetails.Username):
                    await OpenDialog(nameof(UserDetails.Username));
                    break;                
                case nameof(UserDetails.Email):
                    await OpenDialog(nameof(UserDetails.Email));
                    break;                
                default:
                    break;
            }
        }
        private async Task OpenDialog(string property)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

            var parameters = new DialogParameters();
            parameters[nameof(property)] = property;
            parameters[nameof(UserDetails)] = UserDetails;
            parameters[nameof(OnDialogClose)] = EventCallback.Factory.Create(this, OnDialogClose);

            await dialogService.ShowAsync<EditUserPropertiesDialog>(Localizer["EditUserProperty", Localizer[property]], parameters, options);
        }
        private async Task VerifyEmail()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

            var parameters = new DialogParameters();
            parameters[nameof(OnDialogClose)] = EventCallback.Factory.Create(this, OnDialogClose);

            await dialogService.ShowAsync<VerifyEmailDialog>(String.Empty, parameters, options);

        }
        private async Task OnDialogClose()
        {
            await LoadDataToUserDetailsModel();
            StateHasChanged();
        }
    }
}