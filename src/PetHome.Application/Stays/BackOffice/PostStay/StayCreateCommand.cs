using FluentValidation;
using MediatR;
using PetHome.Application.Core;
using PetHome.Application.DTOs;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.Stays.PostStay;

public class StayCreateCommand
{
	public record StayCreateCommandRequest(StayCreateRequest StayCreateRequest)
		: IRequest<Result<Guid>>, ICommandBase;
	
	internal class StayCreateCommandHandler
		: IRequestHandler<StayCreateCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;

		public StayCreateCommandHandler(
			PetHomeDbContext context
		)
		{
			_context = context;
		}
        
		public async Task<Result<Guid>> Handle(
			StayCreateCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var dto = request.StayCreateRequest;
			var stay = new Stay(dto.PetId, dto.CheckInDate, dto.CheckOutDate, dto.DailyRate);
			_context.Add(stay);

			var transaction = new Transaction((decimal)stay.TotalCost!, PaymentMethod.NotSet, stay);
			_context.Add(transaction);

			try
			{
				var result = await _context.SaveChangesAsync(cancellationToken) > 0;
				return result 
					? Result<Guid>.Success(stay.Id)
					: Result<Guid>.Failure("No se pudo insertar la estancia Stay");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			
		}
	}
    
	public class StayCreateCommandRequestValidator
		: AbstractValidator<StayCreateCommandRequest>
	{
		public StayCreateCommandRequestValidator()
		{
			RuleFor(x => x.StayCreateRequest).SetValidator(new StayCreateValidator());
		}
	}
}