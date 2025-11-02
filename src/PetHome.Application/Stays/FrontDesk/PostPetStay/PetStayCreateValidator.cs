using FluentValidation;

namespace PetHome.Application.Stays.FrontDesk.PostPetStay;

public class PetStayCreateValidator: AbstractValidator<PetStayCreateRequest>
{
	public PetStayCreateValidator()
	{
		RuleFor(p => p.DailyRate).NotEmpty()
			.WithMessage("Se debe colocar un costo diario");
		RuleFor(p => p.CheckInDate).NotEmpty()
			.WithMessage("La fecha inicial de estancia no puede estar vacio");
		RuleFor(p => p.CheckOutDate).NotEmpty()
			.WithMessage("La fecha de salida no puede estar vacio");
	}
}