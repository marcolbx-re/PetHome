using FluentValidation;

namespace PetHome.Application.Pets.PatchPet;

public class PetPatchValidator: AbstractValidator<PetPatchRequest>
{
	public PetPatchValidator()
	{
		RuleFor(x => x.OwnerId)
			.NotNull()
			.When(x => x.OwnerId.HasValue)
			.WithMessage("OwnerId no puede ser null");
		
		// Validate Name only when provided in the PATCH
		RuleFor(x => x.Breed)
			.NotEmpty()
			.MinimumLength(2)
			.When(x => x.Breed is not null)
			.WithMessage("La raza debe ser al menos 2 characters.");
		
		RuleFor(x => x.Name)
			.NotEmpty()
			.MinimumLength(1)
			.When(x => x.Name is not null)
			.WithMessage("El nombre debe ser al menos 1 character.");
	}
}