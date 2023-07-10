using address.domain;
using address.persistence;
using MassTransit;

namespace address.core;

public interface DeleteAddress
{
    AddressId AddressId { get; }
}

public interface AddressDeleted
{
    AddressId AddressId { get; }
}

public class DeleteAddressConsumer : IConsumer<DeleteAddress>
{
    private readonly IRequestClient<GetAddress> _getAddress;
    private readonly IWriteRepository<Address> _addressRepository;

    public DeleteAddressConsumer(IWriteRepository<Address> addressRepository, IRequestClient<GetAddress> getAddress)
    {
        _addressRepository = addressRepository;
        _getAddress = getAddress;
    }

    public async Task Consume(ConsumeContext<DeleteAddress> context)
    {
        var addressResponse = await _getAddress.GetResponse<AddressFoundResult, NoAddressFoundResult>(new { context.Message.AddressId });

        if (addressResponse.Is<AddressFoundResult>(out var addressFound))
        {
            var address = addressFound.Message.Address;
            await _addressRepository.Delete(address).ConfigureAwait(false);
            await context.RespondAsync<AddressFoundResult>(new { Address = address }).ConfigureAwait(false);
            await context.Publish<AddressDeleted>(new { address.AddressId }).ConfigureAwait(false);
        }
        else
        {
            await context.RespondAsync<NoAddressFoundResult>(new { }).ConfigureAwait(false);
        }
    }
}