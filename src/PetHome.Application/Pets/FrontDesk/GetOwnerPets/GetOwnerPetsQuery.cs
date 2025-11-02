using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using PetHome.Application.Core;
using PetHome.Application.Pets.GetPet;
using PetHome.Application.Photos.GetPhoto;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.Pets.FrontDesk.GetOwnerPets;

public class GetOwnerPetsQuery
{
	public record GetOwnerPetsQueryRequest
        : IRequest<Result<PagedList<OwnerPetResponse>>>
    {
        public Guid? OwnerId { get; set; }
        public GetPetRequest? Request { get; set; }
    }

    internal class GetOwnerPetsQueryHandler
        : IRequestHandler<GetOwnerPetsQueryRequest, Result<PagedList<OwnerPetResponse>>>
    {
        private readonly PetHomeDbContext _context;
        private readonly IMapper _mapper;

        public GetOwnerPetsQueryHandler(PetHomeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<OwnerPetResponse>>> Handle(
            GetOwnerPetsQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            IQueryable<Pet> queryable = _context.Pets!;

            var predicate = ExpressionBuilder.New<Pet>();
            
            predicate = predicate
                .And(y => y.OwnerId == request.OwnerId);
            if (request.Request!.Type != null)
            {
                predicate = predicate
                    .And(y => y.Type!.Equals(request.Request!.Type));
            }
            if (request.Request.Name != null)
            {
                predicate = predicate
                    .And(y => y.Name!.Contains(request.Request.Name));
            }
            if (request.Request.Breed != null)
            {
                predicate = predicate
                    .And(y => y.Breed! == request.Request.Breed);
            }

            if (!string.IsNullOrEmpty(request.Request.OrderBy))
            {
                Expression<Func<Pet, object>>? orderBySelector =
                    request.Request.OrderBy.ToLower() switch
                    {
                        "type" => pet => pet.Type!,
                        "name" => pet => pet.Name!,
                        "breed" => pet => pet.Breed!,
                        _ => pet => pet!
                    };

                bool orderBy = request.Request.OrderAsc.HasValue
                    ? request.Request.OrderAsc.Value
                    : true;

                queryable = orderBy
                    ? queryable.OrderBy(orderBySelector)
                    : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            IQueryable<OwnerPetResponse> staysQuery = queryable
                .ProjectTo<OwnerPetResponse>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var pagination = await PagedList<OwnerPetResponse>
                .CreateAsync(staysQuery,
                    request.Request.PageNumber,
                    request.Request.PageSize
                );

            return Result<PagedList<OwnerPetResponse>>.Success(pagination);
        }
    }
}

public record OwnerPetResponse(
    Guid? Id,
    string? Name,
    string? Breed,
    PetType? Type,
    List<PhotoResponse>? Photos)
{
    public OwnerPetResponse() : this(null, null, null, null, null)
    {
    }
}