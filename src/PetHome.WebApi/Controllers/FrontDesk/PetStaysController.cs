using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.Stays.FrontDesk.PostPetStay;
using PetHome.Application.Stays.GetPetStayHistory;
using PetHome.Application.Stays.GetStay;
using PetHome.Application.Stays.PostStay;
using PetHome.Domain;

namespace PetHome.WebApi.Controllers.FrontDesk;

[ApiController]
[Route("api/pets/")]
public class PetStaysController : ControllerBase
{
	private readonly ISender _sender;
	public PetStaysController(ISender sender)
	{
		_sender = sender;
	}

	[AllowAnonymous]
	[HttpGet("{petId:guid}/stays")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<PagedList<PetStayResponse>>> PetStaysGet(
		Guid petId,
		[FromQuery] GetStayRequest request,
		CancellationToken cancellationToken
	)
	{
		var query = new GetPetStayHistoryQuery.GetPetStaysQueryRequest{
			Request = request,
			PetId = petId
		};
		Result<PagedList<PetStayResponse>> result =  await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : NotFound();
	}
	
	[Authorize(Policy = PolicyMaster.PET_CREATE)]
	[HttpPost("{petId}/stays")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> StayCreate(
		[FromRoute] Guid petId,
		[FromForm] PetStayCreateRequest request,
		CancellationToken cancellationToken
	)
	{
		var command = new PetStayCreateCommand.PetStayCreateCommandRequest(petId, request);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
}
