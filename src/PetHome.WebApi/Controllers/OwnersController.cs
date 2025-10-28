using System.Net;
using MasterNet.Application.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.Owner.DeleteOwner;
using PetHome.Application.Owner.GetOwner;
using PetHome.Application.Owner.GetOwners;
using PetHome.Application.Owner.OwnerCreate;
using PetHome.Application.Owner.UpdateOwner;
using OwnerResponse = PetHome.Application.Owner.GetOwners.OwnerResponse;

namespace PetHome.WebApi.Controllers;

[ApiController]
[Route("api/owners")]
public class OwnersController : ControllerBase
{
	private readonly ISender _sender;

	public OwnersController(ISender sender)
	{
		_sender = sender;
	}

	[AllowAnonymous]
	[HttpGet]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<PagedList<OwnerResponse>>> PaginationOwner
	(
		[FromQuery] GetOwnersRequest request,
		CancellationToken cancellationToken
	)
	{
		var query = new GetOwnersQuery.GetOwnersQueryRequest {
			OwnerRequest = request
		};
		var result =  await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : NotFound();
	}
	
	[AllowAnonymous]
	[HttpGet("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<OwnerResponse>> CursoGet(
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var query = new GetOwnerQuery.GetOwnerQueryRequest { Id = id };
		var result = await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[AllowAnonymous]
	[HttpPost]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> OwnerCreate(
		[FromForm] OwnerCreateRequest request,
		CancellationToken cancellationToken
	)
	{
		var command = new OwnerCreateCommand.OwnerCreateCommandRequest(request);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[HttpPut("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> CursoUpdate(
		[FromBody] OwnerUpdateRequest request,
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var command = new OwnerUpdateCommand.OwnerUpdateCommandRequest(request, id);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[HttpDelete("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Unit>> OwnerDelete(
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var command = new OwnerDeleteCommand.OwnerDeleteCommandRequest(id);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok() : BadRequest();
	}

}
