using FluentValidation;
using PetHome.Domain;

namespace PetHome.Application.DTOs;

public class PetCreateValidator: AbstractValidator<PetCreateRequest>
{
	public PetCreateValidator()
	{
		RuleFor(x => x.OwnerId).NotEmpty()
			.WithMessage("El Id del owner esta en blanco");
		RuleFor(p => p.Type).NotEmpty().NotEqual(PetType.None)
			.WithMessage("La mascota debe ser un tipo de animal");
	}
}
