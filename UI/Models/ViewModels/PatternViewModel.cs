namespace UI.Models.ViewModels;

public class PatternViewModel
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public double Value_Saves { get; set; }
    public double Value_Fees { get; set; }
    public double Value_Entertainment { get; set; }
}

public class PatternViewModelValidator : AbstractValidator<PatternViewModel>
{
    public PatternViewModelValidator(IStringLocalizer<PatternViewModelValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["NameFieldEmpty"])
            .MinimumLength(4).WithMessage(localizer["NameMinLength"])
            .MaximumLength(50).WithMessage(localizer["NameMaxLength"]);

        RuleFor(x => x.Value_Saves)
            .GreaterThanOrEqualTo(0d).WithMessage(localizer["ValueNegative"]);

        RuleFor(x => x.Value_Fees)
            .GreaterThanOrEqualTo(0d).WithMessage(localizer["ValueNegative"]);

        RuleFor(x => x.Value_Entertainment)
            .GreaterThanOrEqualTo(0d).WithMessage(localizer["ValueNegative"]);
    }
}