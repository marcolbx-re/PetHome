using System.Net;
using MasterNet.Application.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.Owner.GetOwner;
using PetHome.Application.Owner.GetOwners;
using PetHome.Application.Owner.OwnerCreate;
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
		var resultados =  await _sender.Send(query, cancellationToken);
		return resultados.IsSuccess ? Ok(resultados.Value) : NotFound();
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
		var resultado = await _sender.Send(query, cancellationToken);
		return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
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
		var resultado = await _sender.Send(command, cancellationToken);
		return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
	}

}
