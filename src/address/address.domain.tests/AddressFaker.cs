namespace address.domain.tests;

public class AddressFaker : Faker<Address>
{
    public AddressFaker()
    {
        CustomInstantiator(f => new Address(AddressId.From(f.Random.Guid()),
            f.Address.StreetAddress(),
            f.Address.City(),
            f.Address.State(),
            f.Address.ZipCode()
        ));
    }
}