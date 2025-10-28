using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Persistence;

namespace PetHome.Application.Pets.DeletePet;

public class PetDeleteCommand
{
	public record PetDeleteCommandRequest(Guid? Id)
		: IRequest<Result<Unit>>, ICommandBase;

	internal class PetDeleteCommandHandler
		: IRequestHandler<PetDeleteCommandRequest, Result<Unit>>
	{
		private readonly PetHomeDbContext _context;

		public PetDeleteCommandHandler(PetHomeDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Unit>> Handle(
			PetDeleteCommandRequest request,
			CancellationToken cancellationToken
		)
		{
			var pet = await _context.Pets!
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

			if (pet is null)
			{
				return Result<Unit>.Failure("El pet no existe");
			}

			_context.Pets!.Remove(pet);

			var result = await _context.SaveChangesAsync(cancellationToken) > 0;

			return result
				? Result<Unit>.Success(Unit.Value)
				: Result<Unit>.Failure("Error en la transaccion");

		}
	}


	public class PetDeleteCommandRequestValidator
		: AbstractValidator<PetDeleteCommandRequest>
	{
		public PetDeleteCommandRequestValidator()
		{
			RuleFor(x => x.Id).NotNull().WithMessage("No id");
		}
	}

}
