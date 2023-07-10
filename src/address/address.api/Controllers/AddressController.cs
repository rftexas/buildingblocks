using address.api.dto;
using address.core;
using address.domain;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace address.api.Controllers;

[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Address>> GetAll([FromServices] IRequestClient<GetAllAddresses> _requestClient)
    {
        var response = await _requestClient.GetResponse<IEnumerable<Address>>(new { });
        return response.Message;
    }

    [HttpPut]
    public async Task<ActionResult<AddressId>> CreateAddress([FromBody] CreateAddressRequest request, [FromServices] IRequestClient<CreateAddress> _requestClient)
    {
        var response = await _requestClient.GetResponse<Address>(new { request.Street, request.State, request.City, request.PostalCode });
        return Created($"address/{response.Message.AddressId.Value}", response.Message.AddressId);
    }

    [HttpGet("{addressId}")]
    public async Task<ActionResult> GetAddress([FromRoute] AddressId addressId, [FromServices] IRequestClient<GetAddress> _requestClient)
    {
        var response = await _requestClient.GetResponse<NoAddressFoundResult, AddressFoundResult>(new { AddressId = addressId });
        if (response.Is<AddressFoundResult>(out var addressfound))
        {
            return Ok(addressfound.Message.Address);
        }
        return NotFound();
    }

    [HttpPost("{addressId}")]
    public async Task<ActionResult> UpdateAddress([FromRoute] AddressId addressId, [FromServices] IRequestClient<UpdateAddress> _requestClient)
    {
        var response = await _requestClient.GetResponse<NoAddressFoundResult, AddressFoundResult>(new { AddressId = addressId });
        if (response.Is<AddressFoundResult>(out var addressfound))
        {
            return Ok(addressfound.Message.Address);
        }
        return NotFound();
    }

    [HttpDelete("{addressId}")]
    public async Task<ActionResult> DeleteAddress([FromRoute] AddressId addressId, [FromServices] IRequestClient<DeleteAddress> _requestClient)
    {
        var response = await _requestClient.GetResponse<NoAddressFoundResult, AddressFoundResult>(new { AddressId = addressId });
        if (response.Is<AddressFoundResult>(out var addressfound))
        {
            return Ok(addressfound.Message.Address);
        }
        return NotFound();
    }

}