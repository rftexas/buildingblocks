using address.domain;

namespace address.api.dto;

public class CreateAddressRequest : IAddressRequest
{
    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string PostalCode { get; set; }
}