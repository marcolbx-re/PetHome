using FluentValidation;
using MediatR;
using PetHome.Application.Core;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.DTOs;

public class PetCreateCommand
{
	public record PetCreateCommandRequest(PetCreateRequest PetCreateRequest)
		: IRequest<Result<Guid>>, ICommandBase;
	
	internal class PetCreateCommandHandler
		: IRequestHandler<PetCreateCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;
		public PetCreateCommandHandler(
			PetHomeDbContext context
		)
		{
			_context = context;
		}
        
		public async Task<Result<Guid>> Handle(
			PetCreateCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var dto = request.PetCreateRequest;
			var dog = new Pet(dto.Name, dto.Breed, dto.BirthDate, dto.OwnerId,dto.Gender,
				dto.RequiresSpecialDiet, dto.Type, dto.IsDeclawed, dto.Size, dto.SpecialInstructions);
			_context.Add(dog);

			var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;
			return resultado 
				? Result<Guid>.Success(dog.Id)
				: Result<Guid>.Failure("No se pudo insertar el Dog");
		}
	}
    
	public class PetCreateCommandRequestValidator
		: AbstractValidator<PetCreateCommandRequest>
	{
		public PetCreateCommandRequestValidator()
		{
			RuleFor(x => x.PetCreateRequest).SetValidator(new PetCreateValidator());
		}

	}
}
