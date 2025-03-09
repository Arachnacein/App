using FluentValidation;
using Microsoft.Extensions.Localization;

namespace UI.Models.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public double Price { get; set; }
        public TransactionCategoryEnum Category { get; set; }
        public bool IsRecurring { get; set; }
        public bool IsApproved { get; set; }
    }
    public class TransactionViewModelValidator : AbstractValidator<TransactionViewModel>
    {
        public TransactionViewModelValidator(IStringLocalizer<TransactionViewModelValidator> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["NameFieldEmpty"])
                .MinimumLength(3).WithMessage(localizer["NameMinLength"])
                .MaximumLength(30).WithMessage(localizer["NameMaxLength"]);

            RuleFor(x => x.Description)
                .MinimumLength(3).WithMessage(localizer["DescriptionMinLength"])
                .MaximumLength(150).WithMessage(localizer["DescriptionMaxLength"]);

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage(localizer["PriceFieldEmpty"])
                .GreaterThanOrEqualTo(0d).WithMessage(localizer["PriceCorrectValue"])
                .Must(price => !double.IsNaN(price) && !double.IsInfinity(price)).WithMessage(localizer["PriceCorrectValue"]);
        }
    }
}