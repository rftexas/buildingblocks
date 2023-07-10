using MassTransit;
using address.persistence;
using address.domain;

namespace address.core;

public class GetAllAddressesConsumer : IConsumer<GetAllAddresses>
{
    private readonly IReadOnlyRepository<Address> _addressRepository;

    public GetAllAddressesConsumer(IReadOnlyRepository<Address> addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task Consume(ConsumeContext<GetAllAddresses> context)
    {
        var addresses = await _addressRepository.GetAll();
        await context.RespondAsync(addresses).ConfigureAwait(false);
    }
}

public interface GetAllAddresses { }