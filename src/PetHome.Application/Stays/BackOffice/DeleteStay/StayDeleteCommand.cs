using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Persistence;

namespace PetHome.Application.Stays.BackOffice.DeleteStay;

public class StayDeleteCommand
{
	public record StayDeleteCommandRequest(Guid? Id)
		: IRequest<Result<Unit>>, ICommandBase;

	internal class StayDeleteCommandHandler
		: IRequestHandler<StayDeleteCommandRequest, Result<Unit>>
	{
		private readonly PetHomeDbContext _context;

		public StayDeleteCommandHandler(PetHomeDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Unit>> Handle(
			StayDeleteCommandRequest request, 
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

			_context.Stays!.Remove(stay);

			var result = await _context.SaveChangesAsync(cancellationToken) > 0;

			return result 
				? Result<Unit>.Success(Unit.Value) 
				: Result<Unit>.Failure("Error en la transaccion");
        
		}
	}
	
	public class StayDeleteCommandRequestValidator
		: AbstractValidator<StayDeleteCommandRequest>
	{
		public StayDeleteCommandRequestValidator()
		{
			RuleFor(x => x.Id).NotNull().WithMessage("No id");
		}
	}
}