using System.Net;
using MasterNet.Application.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Pets.GetPet;
using PetHome.Application.Pets.GetPets;

namespace PetHome.WebApi.Controllers;

[ApiController]
[Route("api/pets")]
public class PetsController : ControllerBase
{
	private readonly ISender _sender;

	public PetsController(ISender sender)
	{
		_sender = sender;
	}
	
	[AllowAnonymous]
	[HttpGet]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<PagedList<PetResponse>>> PaginationInstructor
	(
		[FromQuery] GetPetRequest request,
		CancellationToken cancellationToken
	)
	{
		var query = new GetPetsQuery.GetPetsQueryRequest {
			PetRequest = request
		};
		var resultados =  await _sender.Send(query, cancellationToken);
		return resultados.IsSuccess ? Ok(resultados.Value) : NotFound();
	}
}
