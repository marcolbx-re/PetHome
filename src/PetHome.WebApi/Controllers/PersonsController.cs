using System.Net;
using MasterNet.Application.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.Persons;
using PetHome.Application.Persons.PersonCreate;

namespace PetHome.WebApi.Controllers;

[ApiController]
[Route("api/persons")]
public class PersonsController : ControllerBase
{
	private readonly ISender _sender;

	public PersonsController(ISender sender)
	{
		_sender = sender;
	}

	[AllowAnonymous]
	[HttpGet]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<PagedList<PersonResponse>>> PaginationPerson
	(
		[FromQuery] GetPersonsRequest request,
		CancellationToken cancellationToken
	)
	{
		var query = new GetPersonsQuery.GetPersonsQueryRequest {
			PersonRequest = request
		};
		var resultados =  await _sender.Send(query, cancellationToken);
		return resultados.IsSuccess ? Ok(resultados.Value) : NotFound();
	}
	
	[AllowAnonymous]
	[HttpPost]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Result<Guid>>> PersonCreate(
		[FromForm] PersonCreateRequest request,
		CancellationToken cancellationToken
	)
	{
		var command = new PersonCreateCommand.PersonCreateCommandRequest(request);
		var resultado = await _sender.Send(command, cancellationToken);
		return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
	}

}
