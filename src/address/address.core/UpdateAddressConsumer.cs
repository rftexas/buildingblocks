using address.domain;
using address.persistence;
using MassTransit;

namespace address.core;

public interface UpdateAddress : IUpdateAddressRequest
{

}

public interface AddressUpdated : IUpdateAddressRequest
{

}

public class UpdateAddressConsumer : IConsumer<UpdateAddress>
{
    public readonly IRequestClient<GetAddress> _getAddress;
    public readonly IWriteRepository<Address> _addressRepository;

    public UpdateAddressConsumer(IWriteRepository<Address> addressRepository, IRequestClient<GetAddress> getAddress)
    {
        _addressRepository = addressRepository;
        _getAddress = getAddress;
    }

    public async Task Consume(ConsumeContext<UpdateAddress> context)
    {
        var addressResponse = await _getAddress.GetResponse<AddressFoundResult, NoAddressFoundResult>(new { context.Message.AddressId }).ConfigureAwait(false);
        if (addressResponse.Is<AddressFoundResult>(out var message))
        {
            var address = message.Message.Address;
            address.Update(context.Message);

            await _addressRepository.Update(address).ConfigureAwait(false);
            await context.RespondAsync<AddressFoundResult>(new { Address = address });
            await context.Publish<AddressUpdated>(context.Message);
        }
        else
        {
            await context.RespondAsync<NoAddressFoundResult>(new { }).ConfigureAwait(false);
        }
    }
}