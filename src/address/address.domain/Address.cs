
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("address.domain.tests")]

namespace address.domain;

public partial class Address
{
    public AddressId AddressId { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }

    internal Address(AddressId addressId, string street, string city, string state, string postalCode)
    {
        AddressId = addressId;
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
    }

    internal static Address From(AddressId addressId, string street, string city, string state, string postalCode)
        => new(addressId, street, city, state, postalCode);
}