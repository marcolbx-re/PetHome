using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.Pets.FrontDesk.GetOwnerPets;
using PetHome.Application.Pets.GetPet;

namespace PetHome.WebApi.Controllers.FrontDesk;

[ApiController]
[Route("api/owners/")]
public class OwnerPetsController : ControllerBase
{
	private readonly ISender _sender;
	public OwnerPetsController(ISender sender)
	{
		_sender = sender;
	}

	[AllowAnonymous]
	[HttpGet("{ownerId:guid}/pets")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<PagedList<OwnerPetResponse>>> PetStaysGet(
		Guid ownerId,
		[FromQuery] GetPetRequest request,
		CancellationToken cancellationToken
	)
	{
		var query = new GetOwnerPetsQuery.GetOwnerPetsQueryRequest
		{
			Request = request,
			OwnerId = ownerId
		};
		Result<PagedList<OwnerPetResponse>> result = await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : NotFound();
	}
}