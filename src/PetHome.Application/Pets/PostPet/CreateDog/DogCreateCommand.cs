using FluentValidation;
using MediatR;
using PetHome.Application.Core;
using PetHome.Application.Owner.OwnerCreate;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.DTOs.CreateDog;

public class DogCreateCommand
{
	public record DogCreateCommandRequest(DogCreateRequest DogCreateRequest)
		: IRequest<Result<Guid>>, ICommandBase;
	
	internal class DogCreateCommandHandler
		: IRequestHandler<DogCreateCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;
		public DogCreateCommandHandler(
			PetHomeDbContext context
		)
		{
			_context = context;
		}
        
		public async Task<Result<Guid>> Handle(
			DogCreateCommand.DogCreateCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var dto = request.DogCreateRequest;
			var dog = new Dog(dto.Name, dto.Breed, dto.BirthDate, dto.OwnerId, dto.Size,
				dto.RequiresExtraExercise, dto.Gender, dto.RequiresSpecialDiet, dto.SpecialInstructions);
			_context.Add(dog);

			var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;
			return resultado 
				? Result<Guid>.Success(dog.Id)
				: Result<Guid>.Failure("No se pudo insertar el Dog");
		}
	}
    
	public class DogCreateCommandRequestValidator
		: AbstractValidator<OwnerCreateCommand.OwnerCreateCommandRequest>
	{
		public DogCreateCommandRequestValidator()
		{
			//RuleFor(x => x.ownerCreateDTO).SetValidator(new OwnerCreateValidator());
		}

	}
}
