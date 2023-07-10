namespace address.domain;

public interface IAddressRequest
{
    string Street { get; }
    string City { get; }
    string State { get; }
    string PostalCode { get; }
}
