using System.Text.RegularExpressions;
using FluentValidation;
namespace address.domain.Validators;

internal partial class AddressRequestValidator : AbstractValidator<IAddressRequest>
{
    public AddressRequestValidator()
    {
        RuleFor(a => a.Street).NotEmpty();
        RuleFor(a => a.City).NotEmpty();
        RuleFor(a => a.State).NotEmpty();
        RuleFor(a => a.PostalCode).NotEmpty().MinimumLength(5).Matches(PostalRegex());
    }

    [GeneratedRegex("\\^d{5}-d{4}|\\d{5}|[A-Z]\\d[A-Z] d[A-Z]\\d$")]
    private static partial Regex PostalRegex();
}