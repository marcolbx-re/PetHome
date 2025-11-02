using System.Net;
using System.Security.Claims;
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

	[Authorize]
	[HttpGet("{ownerId:guid}/pets")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<PagedList<OwnerPetResponse>>> PetStaysGet(
		Guid ownerId,
		[FromQuery] GetPetRequest request,
		CancellationToken cancellationToken
	)
	{
		// Extract ownerId from JWT
		var jwtOwnerId = User.FindFirstValue("ownerId"); // System.Security.Claims
		if (jwtOwnerId == null)
			return Unauthorized("Token is missing ownerId claim.");
		if (ownerId.ToString() != jwtOwnerId)
			return Forbid("You are not authorized to access this owner's pets.");
		var query = new GetOwnerPetsQuery.GetOwnerPetsQueryRequest
		{
			Request = request,
			OwnerId = ownerId
		};
		Result<PagedList<OwnerPetResponse>> result = await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : NotFound();
	}
}