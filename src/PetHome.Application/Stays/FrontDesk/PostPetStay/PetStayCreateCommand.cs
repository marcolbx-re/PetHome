using FluentValidation;
using MediatR;
using PetHome.Application.Core;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.Stays.FrontDesk.PostPetStay;

public class PetStayCreateCommand
{
	public record PetStayCreateCommandRequest(Guid PetId, PetStayCreateRequest StayCreateRequest)
		: IRequest<Result<Guid>>, ICommandBase;
	
	internal class PetStayCreateCommandHandler
		: IRequestHandler<PetStayCreateCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;

		public PetStayCreateCommandHandler(
			PetHomeDbContext context
		)
		{
			_context = context;
		}
        
		public async Task<Result<Guid>> Handle(
			PetStayCreateCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var dto = request.StayCreateRequest;
			var stay = new Stay(request.PetId, dto.CheckInDate, dto.CheckOutDate, dto.DailyRate);
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
    
	public class PetStayCreateCommandRequestValidator
		: AbstractValidator<PetStayCreateCommandRequest>
	{
		public PetStayCreateCommandRequestValidator()
		{
			RuleFor(x => x.StayCreateRequest).SetValidator(new PetStayCreateValidator());
		}
	}
}