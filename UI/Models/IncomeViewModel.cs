using FluentValidation;

namespace UI.Models
{
    public class IncomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime? Date { get; set; }
    }

    internal class IncomeViewModelValidation : AbstractValidator<IncomeViewModel>
    {
        public IncomeViewModelValidation() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name field can't be empty.")
                .MinimumLength(5).WithMessage("Name should be at least 5 characters long.")
                .MaximumLength(50).WithMessage("Name should be less than 50 characters.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Price field can't be empty.")
                .GreaterThanOrEqualTo(0d).WithMessage("Amount na't be lower than 0.")
                .Must(amount => !double.IsNaN(amount) && !double.IsInfinity(amount)).WithMessage("Amount must be a valid number.");
        }
    }
}