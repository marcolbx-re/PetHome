using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.Stays.FrontDesk.DeletePetStay;

public class PetStayDeleteCommand
{
	public record PetStayDeleteCommandRequest(Guid? Id)
		: IRequest<Result<Unit>>, ICommandBase;

	internal class PetStayDeleteCommandHandler
		: IRequestHandler<PetStayDeleteCommandRequest, Result<Unit>>
	{
		private readonly PetHomeDbContext _context;

		public PetStayDeleteCommandHandler(PetHomeDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Unit>> Handle(
			PetStayDeleteCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var stay = await _context.Stays!.
				Include(x => x.Transaction)
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

			if (stay is null)
			{
				return Result<Unit>.Failure("La Stay no existe");
			}
			if (stay.Status == Stay.StayStatus.Active || stay.Status == Stay.StayStatus.Completed)
			{
				return Result<Unit>.Failure("No se puede eliminar una Stay con status: " + stay.Status);
			}

			_context.Stays!.Remove(stay);

			var result = await _context.SaveChangesAsync(cancellationToken) > 0;

			return result 
				? Result<Unit>.Success(Unit.Value) 
				: Result<Unit>.Failure("Error en la transaccion");
        
		}
	}
	
	public class PetStayDeleteCommandRequestValidator
		: AbstractValidator<PetStayDeleteCommandRequest>
	{
		public PetStayDeleteCommandRequestValidator()
		{
			RuleFor(x => x.Id).NotNull().WithMessage("No id");
		}
	}
}