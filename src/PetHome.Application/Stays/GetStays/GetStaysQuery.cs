using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using PetHome.Application.Core;
using PetHome.Application.Stays.GetStay;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.Stays.GetStays;

public class GetStaysQuery
{
    public record GetStaysQueryRequest
        : IRequest<Result<PagedList<StayResponse>>>
    {
        public GetStayRequest? Request { get; set; }
    }

    internal class GetStaysQueryHandler
        : IRequestHandler<GetStaysQuery.GetStaysQueryRequest, Result<PagedList<StayResponse>>>
    {
        private readonly PetHomeDbContext _context;
        private readonly IMapper _mapper;

        public GetStaysQueryHandler(PetHomeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<StayResponse>>> Handle(
            GetStaysQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            IQueryable<Stay> queryable = _context.Stays!;

            var predicate = ExpressionBuilder.New<Stay>();

            if (request.Request!.Status != null)
            {
                predicate = predicate
                    .And(y => y.Status!.Equals(request.Request!.Status));
            }
            if (request.Request.CheckInDate != null)
            {
                predicate = predicate
                    .And(y => y.CheckInDate!.Value == request.Request.CheckInDate);
            }
            if (request.Request.CheckOutDate != null)
            {
                predicate = predicate
                    .And(y => y.CheckOutDate!.Value == request.Request.CheckOutDate);
            }

            if (!string.IsNullOrEmpty(request.Request.OrderBy))
            {
                Expression<Func<Stay, object>>? orderBySelector =
                    request.Request.OrderBy.ToLower() switch
                    {
                        "status" => stay => stay.Status!,
                        "checkIn" => stay => stay.CheckInDate,
                        "checkOut" => stay => stay.CheckOutDate,
                        _ => stay => stay!
                    };

                bool orderBy = request.Request.OrderAsc.HasValue
                    ? request.Request.OrderAsc.Value
                    : true;

                queryable = orderBy
                    ? queryable.OrderBy(orderBySelector)
                    : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            IQueryable<StayResponse> staysQuery = queryable
                .ProjectTo<StayResponse>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var pagination = await PagedList<StayResponse>
                .CreateAsync(staysQuery,
                    request.Request.PageNumber,
                    request.Request.PageSize
                );

            return Result<PagedList<StayResponse>>.Success(pagination);
        }
    }
}
