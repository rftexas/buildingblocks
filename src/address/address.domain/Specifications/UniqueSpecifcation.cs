using Specification.Net;

namespace address.domain.Specifications;

public class UniqueSpecification : Specification<Address>
{
    public UniqueSpecification(AddressId addressId) : base(a => a.AddressId == addressId) { }
}