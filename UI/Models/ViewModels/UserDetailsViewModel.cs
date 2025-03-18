using FluentValidation;
using Microsoft.Extensions.Localization;

namespace UI.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime? AccountCreatedDate { get; set; }
        public DateTime SessionExpiryDate { get; set; }
        public List<string> Roles { get; set; }
        public bool EmailVerified { get; set; }
        public bool Enabled { get; set; }
    }

    public class UserDetailsViewModelValidator : AbstractValidator<UserDetailsViewModel>
    {
        public UserDetailsViewModelValidator(IStringLocalizer<UserDetailsViewModelValidator> localizer)
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage(localizer["UsernameEmpty"]);
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(localizer["FirstNameEmpty"]);
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(localizer["LastNameEmpty"]);
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizer["EmailEmpty"]);       
        }
    }
}