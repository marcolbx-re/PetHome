using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Persistence;

namespace PetHome.Application.Stays.BackOffice.UpdateStay;

public class StayUpdateCommand
{
	public record StayUpdateCommandRequest(
		StayUpdateRequest StayUpdateRequest, 
		Guid? Id
		): IRequest<Result<Guid>>, ICommandBase;

	internal class StayUpdateCommandHandler
		: IRequestHandler<StayUpdateCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;

		public StayUpdateCommandHandler(PetHomeDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Guid>> Handle(
			StayUpdateCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var id = request.Id;

			var stay = await _context.Stays!
				.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            
			if (stay is null)
			{
				return Result<Guid>.Failure("El Stay no existe");
			}

			var updateRequest = request.StayUpdateRequest;
			stay.Status =  updateRequest.Status;
			stay.CheckInDate = updateRequest.CheckInDate;
			stay.CheckOutDate = updateRequest.CheckOutDate;
			stay.DailyRate = updateRequest.DailyRate;
			stay.Notes = updateRequest.Notes;
			stay.TotalCost = updateRequest.TotalCost;

			_context.Entry(stay).State = EntityState.Modified;
			var savedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;

			return savedSuccess 
				? Result<Guid>.Success(stay.Id)
				: Result<Guid>.Failure("Errores en el update de Pet");

		}
	}
	
	public class StayUpdateCommandRequestValidator
		: AbstractValidator<StayUpdateCommandRequest>
	{
		public StayUpdateCommandRequestValidator()
		{
			RuleFor(x => x.StayUpdateRequest).SetValidator(new StayUpdateValidator());
			RuleFor(x => x.Id).NotNull();
		}
	}

}
