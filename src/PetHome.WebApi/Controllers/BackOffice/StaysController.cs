using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.Stays.BackOffice.DeleteStay;
using PetHome.Application.Stays.BackOffice.PatchStay;
using PetHome.Application.Stays.BackOffice.UpdateStay;
using PetHome.Application.Stays.GetStay;
using PetHome.Application.Stays.GetStays;
using PetHome.Application.Stays.PostStay;
using PetHome.Domain;

namespace PetHome.WebApi.Controllers.BackOffice;

[ApiController]
[Route("api/stays")]
public class StaysController : ControllerBase
{
	private readonly ISender _sender;

	public StaysController(ISender sender)
	{
		_sender = sender;
	}
	
	[AllowAnonymous]
	[HttpGet]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<PagedList<StaySimpleResponse>>> PaginationStay
	(
		[FromQuery] GetStayRequest request,
		CancellationToken cancellationToken
	)
	{
		var query = new GetStaysQuery.GetStaysQueryRequest {
			Request = request
		};
		Result<PagedList<StaySimpleResponse>> result =  await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : NotFound();
	}
	
	[AllowAnonymous]
	[HttpGet("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<StayResponse>> StayGet(
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var query = new GetStayQuery.GetStayQueryRequest() { Id = id };
		var result = await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[Authorize(Policy = PolicyMaster.PET_CREATE)]
	[HttpPost]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> StayCreate(
		[FromForm] StayCreateRequest request,
		CancellationToken cancellationToken
	)
	{
		var command = new StayCreateCommand.StayCreateCommandRequest(request);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[Authorize(Policy = PolicyMaster.PET_UPDATE)]
	[HttpPut("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> StayUpdate(
		[FromBody] StayUpdateRequest request,
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var command = new StayUpdateCommand.StayUpdateCommandRequest(request, id);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[Authorize(Policy = PolicyMaster.PET_UPDATE)]
	[HttpPatch("{id}")]
	[Consumes("application/json-patch+json")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> PetPatch(
		Guid id, JsonPatchDocument<StayPatchRequest> patchDoc,
		CancellationToken cancellationToken
	)
	{
		var command = new StayPatchCommand.StayPatchCommandRequest(id, patchDoc);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[Authorize(Policy = PolicyMaster.OWNER_DELETE)]
	[HttpDelete("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Unit>> StayDelete(
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var command = new StayDeleteCommand.StayDeleteCommandRequest(id);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok() : BadRequest();
	}
}
