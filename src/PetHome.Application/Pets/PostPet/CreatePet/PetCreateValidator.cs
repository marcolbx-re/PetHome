using FluentValidation;

namespace PetHome.Application.DTOs;

public class PetCreateValidator: AbstractValidator<PetCreateRequest>
{
	public PetCreateValidator()
	{
		RuleFor(x => x.OwnerId).NotEmpty()
			.WithMessage("El Id del owner esta en blanco");
	}
}
