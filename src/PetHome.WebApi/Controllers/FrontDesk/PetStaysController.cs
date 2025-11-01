using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.Stays.GetPetStayHistory;
using PetHome.Application.Stays.GetStay;

namespace PetHome.WebApi.Controllers.FrontDesk;

[ApiController]
[Route("api/")]
public class PetStaysController : ControllerBase
{
	private readonly ISender _sender;
	public PetStaysController(ISender sender)
	{
		_sender = sender;
	}

	[AllowAnonymous]
	[HttpGet("pets/{petId:guid}/stays")]
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
}
