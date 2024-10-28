using FluentValidation;

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
    internal class TransactionViewModelValidator : AbstractValidator<TransactionViewModel>
    {
        public TransactionViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name field can't be empty.")
                .MinimumLength(5).WithMessage("Name should be at least 5 characters long.")
                .MaximumLength(15).WithMessage("Name should be less than 15 characters.");

            RuleFor(x => x.Description)
                .MinimumLength(5).WithMessage("Description should be at least 5 characters long.")
                .MaximumLength(50).WithMessage("Description should be less than 50 characters.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price field can't be empty.")
                .GreaterThanOrEqualTo(0d).WithMessage("Price na't be lower than 0.")
                .Must(price => !double.IsNaN(price) && !double.IsInfinity(price)).WithMessage("Price must be a valid number.");

        }
    }
}