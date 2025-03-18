using FluentValidation;
using Microsoft.Extensions.Localization;

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
        public RecurringTransactionViewModelValidator(IStringLocalizer<RecurringTransactionViewModelValidator> localizator)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizator["NameFieldEmpty"])
                .MinimumLength(3).WithMessage(localizator["NameMinLength"])
                .MaximumLength(30).WithMessage(localizator["NameMaxLength"]);

            RuleFor(x => x.Description)
                .MinimumLength(3).WithMessage(localizator["DescriptionMinLength"])
                .MaximumLength(150).WithMessage(localizator["DescriptionMaxLength"]);

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage(localizator["PriceFieldEmpty"])
                .GreaterThanOrEqualTo(0d).WithMessage(localizator["PriceCorrectValue"])
                .Must(price => !double.IsNaN(price) && !double.IsInfinity(price)).WithMessage(localizator["PriceCorrectValue"]);

            RuleFor(x => x.Interval)
                .GreaterThan(0).WithMessage(localizator["IntervalCorrectValue"]);

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate).WithMessage(localizator["StartDateLessThanEndDate"]);

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage(localizator["EndDateGreaterThanStartDate"]);

            RuleFor(x => x.MonthlyDay)
                .InclusiveBetween(1, 31).WithMessage(localizator["MonthlyDayCorrectValue"])
                .When(x => x.Frequency == FrequencyEnum.Monthly);

            RuleFor(x => x.YearlyMonth)
                .InclusiveBetween(1, 12).WithMessage(localizator["YearlyMonthCorrectValue"])
                .When(x => x.Frequency == FrequencyEnum.Yearly);

            RuleFor(x => x.YearlyDay)
                .InclusiveBetween(1, 31).WithMessage(localizator["YearlyDayCorrectValue"])
                .When(x => x.Frequency == FrequencyEnum.Yearly);
        }
    }
}