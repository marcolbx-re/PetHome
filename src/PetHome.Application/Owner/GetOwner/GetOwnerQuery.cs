// using AutoMapper;
// using AutoMapper.QueryableExtensions;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using PetHome.Application.Core;
// using PetHome.Domain;
// using PetHome.Persistence;
//
// namespace PetHome.Application.Owner.GetOwner;
//
// public class GetOwnerQuery
// {
// 	public record GetOwnerQueryRequest 
// 		: IRequest<Result<OwnerResponse>>
// 	{
// 		public Guid Id {get;set;}
// 	}
//
// 	internal class GetOwnerQueryHandler
// 		: IRequestHandler<GetOwnerQueryRequest, Result<OwnerResponse>>
// 	{
// 		private readonly PetHomeDbContext _context;
// 		private readonly IMapper _mapper;
//
// 		public GetOwnerQueryHandler(PetHomeDbContext context, IMapper mapper)
// 		{
// 			_context = context;
// 			_mapper = mapper;
// 		}
//
// 		public async Task<Result<OwnerResponse>> Handle(
// 			GetOwnerQueryRequest request, 
// 			CancellationToken cancellationToken
// 		)
// 		{
// 			var curso = await _context.Owners!.Where(x => x.Id == request.Id)
// 				.Include(x=>x.Pets)
// 				// .Include(x => x.Precios)
// 				// .Include(x => x.Photos)
// 				.ProjectTo<OwnerResponse>(_mapper.ConfigurationProvider)
// 				.FirstOrDefaultAsync();
//
// 			return Result<OwnerResponse>.Success(curso!);
// 		}
// 	}
//
//
//
// }
//
// public record OwnerResponse(
// 	Guid Id,
// 	string FirstName,
// 	string LastName,
// 	string Email,
// 	string PhoneNumber,
// 	bool IsNewsletterSubscribed,
// 	DateTime CreatedAt,
// 	List<Pet> Pets
// );
