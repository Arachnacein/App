using FluentValidation;

namespace UI.Extensions
{
    public static class FluentValidationExtension
    {
        /////https://mudblazor.com/components/form#using-fluent-validation
        public static Func<object, string, Task<IEnumerable<string>>> ValidateValue<T>(this AbstractValidator<T> validator)
        => async (model, propertyName) =>
        {
            var result = await validator.ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(erorr => erorr.ErrorMessage);
        };
    }
}