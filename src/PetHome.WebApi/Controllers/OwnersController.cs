using System.Net;
using MasterNet.Application.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Owner.GetOwners;
using PetHome.Application.Persons;

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

}
