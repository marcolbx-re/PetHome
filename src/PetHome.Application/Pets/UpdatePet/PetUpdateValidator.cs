using FluentValidation;

namespace PetHome.Application.Pets.UpdatePet;

public class PetUpdateValidator: AbstractValidator<PetUpdateRequest>
{
	public PetUpdateValidator()
	{
		RuleFor(x => x.OwnerId).NotEmpty()
			.WithMessage("El owner Id esta en blanco");
		RuleFor(x => x.Breed).NotEmpty()
			.WithMessage("La raza no debe ser vacio");
		RuleFor(x => x.Name).NotEmpty()
			.WithMessage("El nombre no debe ser vacio");
	}
}