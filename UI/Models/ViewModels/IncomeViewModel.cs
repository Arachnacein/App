using FluentValidation;
using Microsoft.Extensions.Localization;

namespace UI.Models.ViewModels
{
    public class IncomeViewModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime? Date { get; set; }
    }

    public class IncomeViewModelValidator : AbstractValidator<IncomeViewModel>
    {
        public IncomeViewModelValidator(IStringLocalizer<IncomeViewModelValidator> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["NameFieldEmpty"])
                .MinimumLength(3).WithMessage(localizer["NameMinLength"])
                .MaximumLength(50).WithMessage(localizer["NameMaxLength"]);

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage(localizer["AmountFieldEmpty"])
                .GreaterThanOrEqualTo(0d).WithMessage(localizer["AmountMinValue"])
                .Must(amount => !double.IsNaN(amount) && !double.IsInfinity(amount)).WithMessage(localizer["AmountValidNumber"]);
        }
    }
}