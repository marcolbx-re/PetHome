using System.Net;
using MasterNet.Application.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.DTOs;
using PetHome.Application.DTOs.CreateDog;
using PetHome.Application.Owner.OwnerCreate;
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
	public async Task<ActionResult<PagedList<PetResponse>>> PaginationPet
	(
		[FromQuery] GetPetRequest request,
		CancellationToken cancellationToken
	)
	{
		var query = new GetPetsQuery.GetPetsQueryRequest {
			PetRequest = request
		};
		Result<PagedList<PetResponse>> resultados =  await _sender.Send(query, cancellationToken);
		return resultados.IsSuccess ? Ok(resultados.Value) : NotFound();
	}
	
	[AllowAnonymous]
	[HttpPost("dogs")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> DogCreate(
		[FromForm] DogCreateRequest request,
		CancellationToken cancellationToken
	)
	{
		var command = new DogCreateCommand.DogCreateCommandRequest(request);
		var resultado = await _sender.Send(command, cancellationToken);
		return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
	}
}
