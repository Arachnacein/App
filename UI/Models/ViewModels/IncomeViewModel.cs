using FluentValidation;
using Microsoft.Extensions.Localization;

namespace UI.Models.ViewModels
{
    public class IncomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime? Date { get; set; }
    }

    public class IncomeViewModelValidation : AbstractValidator<IncomeViewModel>
    {
        public IncomeViewModelValidation(IStringLocalizer<IncomeViewModelValidation> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["NameFieldEmpty"])
                .MinimumLength(5).WithMessage(localizer["NameMinLength"])
                .MaximumLength(50).WithMessage(localizer["NameMaxLength"]);

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage(localizer["AmountFieldEmpty"])
                .GreaterThanOrEqualTo(0d).WithMessage(localizer["AmountMinValue"])
                .Must(amount => !double.IsNaN(amount) && !double.IsInfinity(amount)).WithMessage(localizer["AmountValidNumber"]);
        }
    }
}