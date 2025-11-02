using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Core;
using PetHome.Application.DTOs.BackOffice.GetTransaction;
using PetHome.Application.DTOs.BackOffice.GetTransactions;

namespace PetHome.WebApi.Controllers.BackOffice;

[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
	private readonly ISender _sender;

	public TransactionsController(ISender sender)
	{
		_sender = sender;
	}
	
	[AllowAnonymous]
	[HttpGet]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<PagedList<TransactionSimpleResponse>>> PaginationTransactions
	(
		[FromQuery] GetTransactionRequest request,
		CancellationToken cancellationToken
	)
	{
		var query = new GetTransactionsQuery.GetTransactionsQueryRequest {
			Request = request
		};
		Result<PagedList<TransactionSimpleResponse>> result =  await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : NotFound();
	}
	
	[AllowAnonymous]
	[HttpGet("{id}")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<TransactionResponse>> TransactionGet(
		Guid id,
		CancellationToken cancellationToken
	)
	{
		var query = new GetTransactionQuery.GetTransactionQueryRequest() { Id = id };
		var result = await _sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest();
	}
}
