using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Application.Stays.GetStay;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.Stays.GetPetStayHistory;

public class GetPetStayHistoryQuery
{
	public record GetPetStaysQueryRequest
        : IRequest<Result<PagedList<PetStayResponse>>>
    {
        public Guid? PetId { get; set; }
        public GetStayRequest? Request { get; set; }
    }

    internal class GetPetStaysQueryHandler
        : IRequestHandler<GetPetStaysQueryRequest, Result<PagedList<PetStayResponse>>>
    {
        private readonly PetHomeDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetPetStaysQueryHandler(PetHomeDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor =  httpContextAccessor;
        }

        public async Task<Result<PagedList<PetStayResponse>>> Handle(
            GetPetStaysQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            var jwtOwnerId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("ownerId");
            if (jwtOwnerId is null)
                return Result<PagedList<PetStayResponse>>.Failure("Unauthorized");

            // Fetch pet’s owner
            var pet = await _context.Pets
                .Where(p => p.Id == request.PetId)
                .Select(p => new { p.OwnerId })
                .FirstOrDefaultAsync(cancellationToken);

            if (pet is null)
                return Result<PagedList<PetStayResponse>>.Failure("Pet not found");

            if (pet.OwnerId.ToString() != jwtOwnerId)
                return Result<PagedList<PetStayResponse>>.Failure("Forbidden");

            // ✅ Authorized, proceed
            IQueryable<Stay> queryable = _context.Stays!;

            var predicate = ExpressionBuilder.New<Stay>();
            
            predicate = predicate
                .And(y => y.PetId == request.PetId);
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

            IQueryable<PetStayResponse> staysQuery = queryable
                .ProjectTo<PetStayResponse>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var pagination = await PagedList<PetStayResponse>
                .CreateAsync(staysQuery,
                    request.Request.PageNumber,
                    request.Request.PageSize
                );

            return Result<PagedList<PetStayResponse>>.Success(pagination);
        }
    }
}

public record PetStayResponse(
    Guid? Id,
    Guid? PetId,
    DateTime? CheckInDate,
    DateTime? CheckOutDate,
    Stay.StayStatus? Status,
    decimal? DailyRate,
    decimal? TotalCost,
    string? Notes,
    DateTime? CreatedAt,
    Guid? TransactionId)
{
    public PetStayResponse() : this(null, null, null, null, null,null,null,null,null,null)
    {
    }
}