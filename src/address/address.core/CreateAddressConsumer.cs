using address.domain;
using address.persistence;
using MassTransit;

namespace address.core;

public class CreateAddressConsumer : IConsumer<CreateAddress>
{

    private readonly IWriteRepository<Address> _writeAddressRepository;

    public CreateAddressConsumer(IWriteRepository<Address> writeAddressRepository)
    {
        _writeAddressRepository = writeAddressRepository;
    }

    public async Task Consume(ConsumeContext<CreateAddress> context)
    {
        var address = Address.Create(context.Message);
        await _writeAddressRepository.Create(address).ConfigureAwait(false);
        await context.RespondAsync(address).ConfigureAwait(false);
        await context.Publish<AddressCreated>(address).ConfigureAwait(false);
    }
}

public interface CreateAddress : ICreateAddressRequest { }
public interface AddressCreated
{
    AddressId AddressId { get; }
    string Street { get; }
    string City { get; }
    string State { get; }
    string PostalCode { get; }
}