using FluentValidation;

namespace PetHome.Application.Accounts.RegisterAsOwner;

public class RegisterAsOwnerValidator : AbstractValidator<RegisterAsOwnerRequest>
{
	public RegisterAsOwnerValidator()
	{
		RuleFor(x => x.Email).NotEmpty()
			.WithMessage("El Email no es correcto");
		RuleFor(x => x.Password).NotEmpty()
			.WithMessage("El password esta en blanco");
		RuleFor(x => x.FirstName).NotEmpty()
			.WithMessage("El nombre esta en blanco");
		RuleFor(x => x.LastName).NotEmpty()
			.WithMessage("El apellido esta en blanco");
		RuleFor(x => x.IdentificationNumber).NotEmpty()
			.WithMessage("El Id esta en blanco");
		RuleFor(x => x.IdentificationType).NotEmpty()
			.WithMessage("El tipo de Id esta en blanco");
		RuleFor(x => x.PhoneNumber).NotEmpty()
			.WithMessage("El telefono esta en blanco");
	}
}