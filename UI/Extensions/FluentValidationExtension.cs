namespace UI.Extensions;

public static class FluentValidationExtension
{
    /////https://mudblazor.com/components/form#using-fluent-validation
    /// <summary>
    ///     FluentValidation extension method for validating a specific property of a model asynchronously. 
    ///     This method allows you to validate a single property of a model using FluentValidation and returns any validation errors as an enumerable of strings.
    /// </summary>
    /// <typeparam name="T">The type of the model to validate.</typeparam>
    /// <param name="validator">The validator instance.</param>
    /// <returns>A function that validates a property and returns validation errors.</returns>
    public static Func<object, string, Task<IEnumerable<string>>> ValidateValue<T>(this AbstractValidator<T> validator)
        => async (model, propertyName) =>
        {
            var result = await validator.ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(erorr => erorr.ErrorMessage);
        };
}