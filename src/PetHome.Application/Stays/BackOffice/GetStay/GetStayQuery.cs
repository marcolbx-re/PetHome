using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.Stays.GetStay;

public class GetStayQuery
{
	public record GetStayQueryRequest
		: IRequest<Result<StayResponse>>
	{
		public GetStayRequest? Request {get;set;}
		public Guid Id {get;set;}
	}
	
	internal class GetStayQueryHandler
		: IRequestHandler<GetStayQueryRequest, Result<StayResponse>>
	{
		private readonly PetHomeDbContext _context;
		private readonly IMapper _mapper;

		public GetStayQueryHandler(PetHomeDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<Result<StayResponse>> Handle(
			GetStayQueryRequest request, 
			CancellationToken cancellationToken
		)
		{
			var stay = await _context.Stays!.Where(s => s.Id == request.Id)
				.ProjectTo<StayResponse>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(cancellationToken: cancellationToken);

			return Result<StayResponse>.Success(stay!);
		}
	}
}

public record StayResponse(
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
	public StayResponse() : this(null, null, null, null, null,null,null,null,null,null)
	{
	}
}

