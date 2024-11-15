using FluentValidation;
using Microsoft.Extensions.Localization;

namespace UI.Models.ViewModels
{
    public class RegistrationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationViewModelValidator(IStringLocalizer<RegistrationViewModelValidator> localizer)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(localizer["FirstNameFieldEmpty"])
                .MinimumLength(2).WithMessage(localizer["FirstNameMinLength"])
                .MaximumLength(50).WithMessage(localizer["FirstNameMaxLength"]);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(localizer["LastNameFieldEmpty"])
                .MinimumLength(2).WithMessage(localizer["LastNameMinLength"])
                .MaximumLength(50).WithMessage(localizer["LastNameMaxLength"]);

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage(localizer["UsernameFieldEmpty"])
                .MinimumLength(3).WithMessage(localizer["UsernameMinLength"])
                .MaximumLength(30).WithMessage(localizer["UsernameMaxLength"])
                .Matches("^[a-zA-Z0-9]*$").WithMessage(localizer["UsernameSpecialCharacters"]);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizer["EmailFieldEmpty"])
                .EmailAddress().WithMessage(localizer["EmailInvalid"])
                .MaximumLength(100).WithMessage(localizer["EmailMaxLength"]); 

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(localizer["PasswordFieldEmpty"])
                .MinimumLength(8).WithMessage(localizer["PasswordMinLength"])
                .MaximumLength(100).WithMessage(localizer["PasswordMaxLength"])
                .Matches("[A-Z]").WithMessage(localizer["PasswordUppercase"])
                .Matches("[a-z]").WithMessage(localizer["PasswordLowercase"])
                .Matches("[0-9]").WithMessage(localizer["PasswordDigit"])
                .Matches("[^a-zA-Z0-9]").WithMessage(localizer["PasswordSpecial"]);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(localizer["ConfirmPasswordFieldEmpty"])
                .Equal(x => x.Password).WithMessage(localizer["PasswordsDoNotMatch"]);
        }
    }
}