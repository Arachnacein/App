using FluentValidation;
using Microsoft.Extensions.Localization;

namespace UI.Models.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public double Price { get; set; }
        public TransactionCategoryEnum Category { get; set; }
    }
    public class TransactionViewModelValidator : AbstractValidator<TransactionViewModel>
    {
        public TransactionViewModelValidator(IStringLocalizer<TransactionViewModelValidator> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["NameFieldEmpty"])
                .MinimumLength(5).WithMessage(localizer["NameMinLength"])
                .MaximumLength(15).WithMessage(localizer["NameMaxLength"]);

            RuleFor(x => x.Description)
                .MinimumLength(5).WithMessage(localizer["DescriptionMinLength"])
                .MaximumLength(50).WithMessage(localizer["DescriptionMaxLength"]);

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage(localizer["PriceFieldEmpty"])
                .GreaterThanOrEqualTo(0d).WithMessage(localizer["PriceCorrectValue"])
                .Must(price => !double.IsNaN(price) && !double.IsInfinity(price)).WithMessage(localizer["PriceCorrectValue"]);

        }
    }
}