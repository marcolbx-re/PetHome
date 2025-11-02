using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using PetHome.Application.Core;
using PetHome.Application.DTOs.BackOffice.GetTransaction;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.DTOs.BackOffice.GetTransactions;

public class GetTransactionsQuery
{
	public record GetTransactionsQueryRequest
        : IRequest<Result<PagedList<TransactionSimpleResponse>>>
    {
        public GetTransactionRequest? Request { get; set; }
    }

    internal class GetTransactionsQueryHandler
        : IRequestHandler<GetTransactionsQueryRequest, Result<PagedList<TransactionSimpleResponse>>>
    {
        private readonly PetHomeDbContext _context;
        private readonly IMapper _mapper;

        public GetTransactionsQueryHandler(PetHomeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<TransactionSimpleResponse>>> Handle(
            GetTransactionsQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            IQueryable<Transaction> queryable = _context.Transactions!;

            var predicate = ExpressionBuilder.New<Transaction>();

            if (request.Request!.Status != null)
            {
                predicate = predicate
                    .And(y => y.Status!.Equals(request.Request!.Status));
            }
            
            if (request.Request!.CreatedAt != null)
            {
                predicate = predicate
                    .And(y => y.CreatedAt!.Value.Date == request.Request.CreatedAt.Value.Date);
            }

            if (!string.IsNullOrEmpty(request.Request.OrderBy))
            {
                Expression<Func<Transaction, object>>? orderBySelector =
                    request.Request.OrderBy.ToLower() switch
                    {
                        "status" => transaction => transaction.Status!,
                        "createdAt" => transaction => transaction.CreatedAt!,
                        _ => transaction => transaction!
                    };

                bool orderBy = request.Request.OrderAsc.HasValue
                    ? request.Request.OrderAsc.Value
                    : true;

                queryable = orderBy
                    ? queryable.OrderBy(orderBySelector)
                    : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            IQueryable<TransactionSimpleResponse> transactionsQuery = queryable
                .ProjectTo<TransactionSimpleResponse>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var pagination = await PagedList<TransactionSimpleResponse>
                .CreateAsync(transactionsQuery,
                    request.Request.PageNumber,
                    request.Request.PageSize
                );

            return Result<PagedList<TransactionSimpleResponse>>.Success(pagination);
        }
    }
}

public record TransactionSimpleResponse(
    Guid? Id,
    Guid? StayId,
    PaymentMethod? PaymentMethod,
    DateTime? CreatedAt,
    TransactionStatus? Status)
{
    public TransactionSimpleResponse() : this(null, null, null, null, null)
    {
    }
}
