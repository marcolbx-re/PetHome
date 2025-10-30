using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PetHome.Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Pets.GetPet;
using PetHome.Domain;
using PetHome.Persistence;
using ExpressionBuilder = PetHome.Application.Core.ExpressionBuilder;

namespace PetHome.Application.Pets.GetPets;

public class GetPetsQuery
{
	public record GetPetsQueryRequest
		: IRequest<Result<PagedList<PetSimpleResponse>>>
	{
		public GetPetRequest? PetRequest {get;set;}
	}
	
	internal class GetPetsQueryHandler
    : IRequestHandler<GetPetsQueryRequest, Result<PagedList<PetSimpleResponse>>>
    {
        private readonly PetHomeDbContext _context;
        private readonly IMapper _mapper;

        public GetPetsQueryHandler(PetHomeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<PetSimpleResponse>>> Handle(
            GetPetsQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            IQueryable<Pet> queryable = _context.Pets!;

            var predicate = ExpressionBuilder.New<Pet>();
            if(!string.IsNullOrEmpty(request.PetRequest!.Name))
            {
                predicate = predicate
                .And(y => y.Name!.Contains(request.PetRequest!.Name));
            }

            if(!string.IsNullOrEmpty(request.PetRequest!.Breed))
            {
                predicate = predicate
                .And(y => y.Breed!.Contains(request.PetRequest!.Breed));
            }
            
            if(request.PetRequest!.Type != null)
            {
                if (request.PetRequest!.Type != PetType.None)
                {
                    predicate = predicate
                        .And(y => y.Type!.Equals(request.PetRequest!.Type));
                }
            }

            if(!string.IsNullOrEmpty(request.PetRequest.OrderBy))
            {
                Expression<Func<Pet, object>>? orderBySelector = 
                request.PetRequest.OrderBy.ToLower() switch 
                {
                    "name" => pet => pet.Name!,
                    "breed" => pet => pet.Breed!,
                    "type" => pet => pet.Type!,
                    _ => pet => pet.Name!
                };

                bool orderBy = request.PetRequest.OrderAsc.HasValue 
                            ? request.PetRequest.OrderAsc.Value
                            : true;

                queryable = orderBy 
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            IQueryable<PetSimpleResponse> petsQuery = queryable
                    .Include(x => x.Photos)
                        .ProjectTo<PetSimpleResponse>(_mapper.ConfigurationProvider)
                        .AsQueryable();

            var pagination = await PagedList<PetSimpleResponse>
                .CreateAsync(petsQuery, 
                request.PetRequest.PageNumber,
                request.PetRequest.PageSize
                );

            return Result<PagedList<PetSimpleResponse>>.Success(pagination);
        }
    }
}