using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Persistence;

namespace PetHome.Application.Owner.DeleteOwner;

public class OwnerDeleteCommand
{
	public record OwnerDeleteCommandRequest(Guid? Id)
		: IRequest<Result<Unit>>, ICommandBase;

	internal class OwnerDeleteCommandHandler
		: IRequestHandler<OwnerDeleteCommandRequest, Result<Unit>>
	{
		private readonly PetHomeDbContext _context;

		public OwnerDeleteCommandHandler(PetHomeDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Unit>> Handle(
			OwnerDeleteCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var owner = await _context.Owners!.
				Include(x => x.Pets)
				.FirstOrDefaultAsync(x => x.Id == request.Id);

			if (owner is null)
			{
				return Result<Unit>.Failure("El owner no existe");
			}

			_context.Owners!.Remove(owner);

			var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;

			return resultado 
				? Result<Unit>.Success(Unit.Value) 
				: Result<Unit>.Failure("Error en la transaccion");
        
		}
	}


	public class OwnerDeleteCommandRequestValidator
		: AbstractValidator<OwnerDeleteCommandRequest>
	{
		public OwnerDeleteCommandRequestValidator()
		{
			RuleFor(x => x.Id).NotNull().WithMessage("No id");
		}
	}


}