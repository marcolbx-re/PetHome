using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.DTOs.BackOffice.GetTransaction;

public class GetTransactionQuery
{
	public record GetTransactionQueryRequest
		: IRequest<Result<TransactionResponse>>
	{
		public GetTransactionRequest? Request {get;set;}
		public Guid Id {get;set;}
	}
	
	internal class GetTransactionQueryHandler
		: IRequestHandler<GetTransactionQueryRequest, Result<TransactionResponse>>
	{
		private readonly PetHomeDbContext _context;
		private readonly IMapper _mapper;

		public GetTransactionQueryHandler(PetHomeDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<Result<TransactionResponse>> Handle(
			GetTransactionQueryRequest request, 
			CancellationToken cancellationToken
		)
		{
			var transaction = await _context.Transactions!.Where(s => s.Id == request.Id)
				.ProjectTo<TransactionResponse>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(cancellationToken: cancellationToken);

			return Result<TransactionResponse>.Success(transaction!);
		}
	}
}

public record TransactionResponse(
	Guid? StayId,
	decimal? Amount,
	PaymentMethod? PaymentMethod,
	DateTime? CreatedAt,
	TransactionStatus? Status,
	string? Reference,
	Guid? TransactionId)
{
	public TransactionResponse() : this(null, null, null, null, null,null,null)
	{
	}
}

