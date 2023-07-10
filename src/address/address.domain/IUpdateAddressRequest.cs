namespace address.domain;

public interface IUpdateAddressRequest : IAddressRequest
{
    AddressId AddressId { get; }
}
