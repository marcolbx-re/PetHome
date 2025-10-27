using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MasterNet.Application.Core;
using MediatR;
using PetHome.Application.Core;
using PetHome.Persistence;
using PetHome.Persistence.Test;

namespace PetHome.Application.Persons;

public class GetPersonsQuery
{
	public record GetPersonsQueryRequest
    : IRequest<Result<PagedList<PersonResponse>>>
    {
        public GetPersonsRequest? PersonRequest {get;set;}

    }


    internal class GetPersonsQueryHandler
    : IRequestHandler<GetPersonsQueryRequest, Result<PagedList<PersonResponse>>>
    {
        private readonly PetHomeDbContext _context;
        private readonly IMapper _mapper;

        public GetPersonsQueryHandler(PetHomeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<PersonResponse>>> Handle(
            GetPersonsQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
           
            IQueryable<Person> queryable = _context.Persons!;

            var predicate = ExpressionBuilder.New<Person>();
            if(!string.IsNullOrEmpty(request.PersonRequest!.FirstName))
            {
                predicate = predicate
                .And(y => y.FirstName!.Contains(request.PersonRequest!.FirstName));
            }

            if(!string.IsNullOrEmpty(request.PersonRequest!.LastName))
            {
                predicate = predicate
                    .And(y => y.LastName!.Contains(request.PersonRequest!.LastName));
            }

            if(!string.IsNullOrEmpty(request.PersonRequest.OrderBy))
            {
                Expression<Func<Person, object>>? orderBySelector = 
                request.PersonRequest.OrderBy.ToLower() switch 
                {
                    "name" => instructor => instructor.FirstName!,
                    "lastname" => instructor => instructor.LastName!,
                    _ => instructor => instructor.FirstName!
                };

                bool orderBy = request.PersonRequest.OrderAsc.HasValue 
                            ? request.PersonRequest.OrderAsc.Value
                            : true;

                queryable = orderBy 
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            var instructoresQuery = queryable
                        .ProjectTo<PersonResponse>(_mapper.ConfigurationProvider)
                        .AsQueryable();

            var pagination = await PagedList<PersonResponse>
                .CreateAsync(instructoresQuery, 
                request.PersonRequest.PageNumber,
                request.PersonRequest.PageSize
                );

            return Result<PagedList<PersonResponse>>.Success(pagination);
        }
    }
}




public record PersonResponse(
    Guid? Id,
    string? FirstName,
    string? LastName
)
{
    public PersonResponse() : this(null, null, null)
    {
    }
}
