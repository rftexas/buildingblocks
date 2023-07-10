using address.domain;
using address.domain.Specifications;
using address.persistence;
using MassTransit;

namespace address.core;

public interface GetAddress
{
    AddressId AddressId { get; }
}

public interface NoAddressFoundResult { }

public interface AddressFoundResult
{
    Address Address { get; }
}

public class GetAddressConsumer : IConsumer<GetAddress>
{
    public readonly IReadOnlyRepository<Address> _addressRepository;

    public GetAddressConsumer(IReadOnlyRepository<Address> addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task Consume(ConsumeContext<GetAddress> context)
    {
        var spec = new UniqueSpecification(context.Message.AddressId);

        var address = await _addressRepository.Get(spec).ConfigureAwait(false);

        if (address != null)
        {
            await context.RespondAsync<AddressFoundResult>(new { Address = address }).ConfigureAwait(false);
        }
        else
        {
            await context.RespondAsync<NoAddressFoundResult>(new { }).ConfigureAwait(false);
        }
    }
}