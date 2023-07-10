using address.domain.Validators;
using FluentValidation;

namespace address.domain;

public partial class Address
{
    public static Address Create(ICreateAddressRequest request)
    {
        new AddressRequestValidator().Validate(request, options => options.ThrowOnFailures());
        return From(AddressId.From(Guid.Empty), request.Street, request.City, request.State, request.PostalCode);
    }

    public Address Update(IAddressRequest request)
    {
        new AddressRequestValidator().ValidateAndThrow(request);
        if (!request.Street.Equals(Street))
        {
            Street = request.Street;
        }
        if (!request.City.Equals(City))
        {
            City = request.City;
        }
        if (!request.State.Equals(State))
        {
            State = request.State;
        }
        if (!request.PostalCode.Equals(PostalCode))
        {
            PostalCode = request.PostalCode;
        }

        return this;
    }
}
