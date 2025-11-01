using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.Stays.GetStay;
using PetHome.Application.Stays.GetStays;
using PetHome.Application.Stays.PostStay;
using PetHome.Domain;

namespace PetHome.WebApi.Controllers.BackOffice;

[ApiController]
[Route("api")]
public class StaysController : ControllerBase
{
	private readonly ISender _sender;

	public StaysController(ISender sender)
	{
		_sender = sender;
	}
	
	[AllowAnonymous]
	[HttpGet("stays")]
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
	
	[Authorize(Policy = PolicyMaster.PET_CREATE)]
	[HttpPost("/pets/{petId}/stays")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> StayCreate(
		[FromRoute] Guid petId,
		[FromForm] StayCreateRequest request,
		CancellationToken cancellationToken
	)
	{
		var command = new StayCreateCommand.StayCreateCommandRequest(petId, request);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
}
