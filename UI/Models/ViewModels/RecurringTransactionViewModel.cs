using FluentValidation;

namespace UI.Models.ViewModels
{
    public class RecurringTransactionViewModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public FrequencyEnum Frequency { get; set; }
        public int Interval { get; set; }
        public List<DayOfWeek>? WeeklyDays { get; set; }
        public int? MonthlyDay { get; set; } //if Frequency = Monthly then this is the day of the month
        public int? YearlyMonth { get; set; } // if Frequency = yearly then this is the month of the year
        public int? YearlyDay { get; set; } // if Frequency = yearly then this is the day of the month
        public int? MaxOccurrences { get; set; }
    }

    public class RecurringTransactionViewModelValidator : AbstractValidator<RecurringTransactionViewModel>
    {
        public RecurringTransactionViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("NameFieldEmpty")
                .MinimumLength(3).WithMessage("NameMinLength")
                .MaximumLength(30).WithMessage("NameMaxLength");

            RuleFor(x => x.Description)
                .MinimumLength(3).WithMessage("DescriptionMinLength")
                .MaximumLength(150).WithMessage("DescriptionMaxLength")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("PriceFieldEmpty")
                .GreaterThanOrEqualTo(0d).WithMessage("PriceCorrectValue")
                .Must(price => !double.IsNaN(price) && !double.IsInfinity(price)).WithMessage("PriceCorrectValue");

            RuleFor(x => x.Interval)
                .GreaterThan(0).WithMessage("IntervalCorrectValue");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("StartDateLessThanEndDate");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("EndDateGreaterThanStartDate");

            RuleFor(x => x.MonthlyDay)
                .InclusiveBetween(1, 31).WithMessage("MonthlyDayCorrectValue")
                .When(x => x.Frequency == FrequencyEnum.Monthly);

            RuleFor(x => x.YearlyMonth)
                .InclusiveBetween(1, 12).WithMessage("YearlyMonthCorrectValue")
                .When(x => x.Frequency == FrequencyEnum.Yearly);

            RuleFor(x => x.YearlyDay)
                .InclusiveBetween(1, 31).WithMessage("YearlyDayCorrectValue")
                .When(x => x.Frequency == FrequencyEnum.Yearly);
        }
    }
}