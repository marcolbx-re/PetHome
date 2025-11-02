using FluentValidation;
using PetHome.Application.Stays.PostStay;

namespace PetHome.Application.DTOs;

public class StayCreateValidator: AbstractValidator<StayCreateRequest>
{
	public StayCreateValidator()
	{
		RuleFor(p => p.PetId).NotEmpty()
			.WithMessage("Se debe colocar un PetId");
		RuleFor(p => p.DailyRate).NotEmpty()
			.WithMessage("Se debe colocar un costo diario");
		RuleFor(p => p.CheckInDate).NotEmpty()
			.WithMessage("La fecha inicial de estancia no puede estar vacio");
		RuleFor(p => p.CheckOutDate).NotEmpty()
			.WithMessage("La fecha de salida no puede estar vacio");
	}
}