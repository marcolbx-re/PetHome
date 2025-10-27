using FluentValidation;

namespace PetHome.Application.Owner.OwnerCreate;

public class OwnerCreateValidator : AbstractValidator<OwnerCreateRequest>
{
	public OwnerCreateValidator()
	{
		// RuleFor(x => x.FirstName).NotEmpty()
		// 	.WithMessage("El nombre esta en blanco");
		//
		// RuleFor(x => x.LastName).NotEmpty()
		// 	.WithMessage("El apellido esta en blanco");
	}
}
