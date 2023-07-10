namespace address.domain.tests;

public class AddressTests
{
    [Fact]
    public void AddressCreate_validates()
    {
        var addressRequest = Mock.Of<ICreateAddressRequest>();
        addressRequest.Invoking(a => Address.Create(a)).Should().Throw<FluentValidation.ValidationException>();
    }

    [Fact]
    public void AddressCreate_works()
    {
        var shape = new AddressFaker().Generate();

        var addressRequest = Mock.Of<ICreateAddressRequest>(r =>
            r.PostalCode == shape.PostalCode &&
            r.State == shape.State &&
            r.City == shape.City &&
            r.Street == shape.Street
        );

        Address result = Address.Create(addressRequest);

        result.AddressId.Should().NotBeNull();
        result.City.Should().NotBeNull().And.Be(shape.City);
        result.State.Should().NotBeNull().And.Be(shape.State);
        result.PostalCode.Should().NotBeNull().And.Be(shape.PostalCode);
    }

    [Fact]
    public void AddressUpdate_validates()
    {
        var address = new AddressFaker().Generate();
        var addressRequest = Mock.Of<IUpdateAddressRequest>();
        address.Invoking(a => a.Update(addressRequest)).Should().Throw<FluentValidation.ValidationException>();
    }

    [Fact]
    public void AddressUpdate_works()
    {
        var faker = new AddressFaker();
        var shape = faker.Generate();
        var address = faker.Generate();

        var addressRequest = Mock.Of<IUpdateAddressRequest>(r =>
            r.PostalCode == shape.PostalCode &&
            r.State == shape.State &&
            r.City == shape.City &&
            r.Street == shape.Street &&
            r.AddressId == address.AddressId
        );

        address.Update(addressRequest);

        address.AddressId.Should().NotBeNull();
        address.City.Should().NotBeNull().And.Be(shape.City);
        address.State.Should().NotBeNull().And.Be(shape.State);
        address.PostalCode.Should().NotBeNull().And.Be(shape.PostalCode);
    }
}