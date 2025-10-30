using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.DTOs;
using PetHome.Application.Pets.DeletePet;
using PetHome.Application.Pets.GetPet;
using PetHome.Application.Pets.GetPets;
using PetHome.Application.Pets.UpdatePet;
using PetHome.Domain;

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
	public async Task<ActionResult<PagedList<PetDTO>>> PaginationPet
	(
		[FromQuery] GetPetRequest request,
		CancellationToken cancellationToken
	)
	{
		var query = new GetPetsQuery.GetPetsQueryRequest {
			PetRequest = request
		};
		Result<PagedList<PetSimpleResponse>> result =  await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : NotFound();
	}
	
	[Authorize(Policy = PolicyMaster.PET_CREATE)]
	[HttpPost]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> CatCreate(
		[FromForm] PetCreateRequest request,
		CancellationToken cancellationToken
	)
	{
		var command = new PetCreateCommand.PetCreateCommandRequest(request);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[AllowAnonymous]
	[HttpGet("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<CatDTO>> CursoGet(
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var query = new GetPetQuery.GetPetQueryRequest{ Id = id };
		var result = await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[Authorize(Policy = PolicyMaster.PET_UPDATE)]
	[HttpPut("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> CursoUpdate(
		[FromBody] PetUpdateRequest request,
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var command = new PetUpdateCommand.PetUpdateCommandRequest(request, id);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
	
	[Authorize(Policy = PolicyMaster.PET_DELETE)]
	[HttpDelete("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Unit>> OwnerDelete(
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var command = new PetDeleteCommand.PetDeleteCommandRequest(id);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok() : BadRequest();
	}
}
