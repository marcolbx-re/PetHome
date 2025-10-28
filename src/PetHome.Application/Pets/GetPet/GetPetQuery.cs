using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Application.DTOs;
using PetHome.Persistence;

namespace PetHome.Application.Pets.GetPet;

public class GetPetQuery
{
	public record GetPetQueryRequest
		: IRequest<Result<PetDTO>>
	{
		public Guid Id {get;set;}
	}
	
	internal class GetPetQueryHandler
		: IRequestHandler<GetPetQueryRequest, Result<PetDTO>>
	{
		private readonly PetHomeDbContext _context;
		private readonly IMapper _mapper;

		public GetPetQueryHandler(PetHomeDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<Result<PetDTO>> Handle(
			GetPetQueryRequest request, 
			CancellationToken cancellationToken
		)
		{
			var pet = await _context.Pets!.Where(x => x.Id == request.Id)
				.ProjectTo<PetDTO>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(cancellationToken: cancellationToken);

			return Result<PetDTO>.Success(pet!);
		}
	}
}

