using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MasterNet.Application.Core;
using MediatR;
using PetHome.Application.Core;
using PetHome.Persistence;

namespace PetHome.Application.Owner.GetOwners;

public class GetOwnersQuery
{

    public record GetOwnersQueryRequest
    : IRequest<Result<PagedList<OwnerResponse>>>
    {
        public GetOwnersRequest? OwnerRequest {get;set;}

    }


    internal class GetOwnersQueryHandler
    : IRequestHandler<GetOwnersQueryRequest, Result<PagedList<OwnerResponse>>>
    {
        private readonly PetHomeDbContext _context;
        private readonly IMapper _mapper;

        public GetOwnersQueryHandler(PetHomeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<OwnerResponse>>> Handle(
            GetOwnersQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
           
            IQueryable<Domain.Owner> queryable = _context.Owners!;

            var predicate = ExpressionBuilder.New<Domain.Owner>();
            
            if(!string.IsNullOrEmpty(request.OwnerRequest!.IdentificationNumber))
            {
                predicate = predicate
                    .And(y => y.IdentificationNumber!.Contains(request.OwnerRequest!.IdentificationNumber));
            }
            
            if(!string.IsNullOrEmpty(request.OwnerRequest!.FirstName))
            {
                predicate = predicate
                .And(y => y.FirstName!.Contains(request.OwnerRequest!.FirstName));
            }

            if(!string.IsNullOrEmpty(request.OwnerRequest!.LastName))
            {
                predicate = predicate
                .And(y => y.LastName!.Contains(request.OwnerRequest!.LastName));
            }

            if(!string.IsNullOrEmpty(request.OwnerRequest.OrderBy))
            {
                Expression<Func<Domain.Owner, object>>? orderBySelector = 
                request.OwnerRequest.OrderBy.ToLower() switch 
                {
                    "firstName" => owner => owner.FirstName!,
                    "lastName" => owner => owner.LastName!,
                    _ => owner => owner.FirstName!
                };

                bool orderBy = request.OwnerRequest.OrderAsc.HasValue 
                            ? request.OwnerRequest.OrderAsc.Value
                            : true;

                queryable = orderBy 
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            var instructoresQuery = queryable
                        .ProjectTo<OwnerResponse>(_mapper.ConfigurationProvider)
                        .AsQueryable();

            var pagination = await PagedList<OwnerResponse>
                .CreateAsync(instructoresQuery, 
                request.OwnerRequest.PageNumber,
                request.OwnerRequest.PageSize
                );

            return Result<PagedList<OwnerResponse>>.Success(pagination);
        }
    }
}




public record OwnerResponse(
    Guid? Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    bool IsNewsletterSubscribed,
    string? IdentificationType,
    string? IdentificationNumber
)
{
    public OwnerResponse() : this(null, null, null, null, null, false, null,null)
    {
    }
}
