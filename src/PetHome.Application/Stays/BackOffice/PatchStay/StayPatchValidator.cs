using FluentValidation;

namespace PetHome.Application.Stays.BackOffice.PatchStay;

public class StayPatchValidator : AbstractValidator<StayPatchRequest>
{
	public StayPatchValidator()
	{
		RuleFor(x => x.CheckInDate)
			.LessThan(x => x.CheckOutDate.Value)
			.When(x => x.CheckInDate.HasValue && x.CheckOutDate.HasValue)
			.WithMessage("CheckInDate must be before CheckOutDate.");

		RuleFor(x => x.DailyRate)
			.GreaterThan(0)
			.When(x => x.DailyRate.HasValue)
			.WithMessage("DailyRate must be positive.");

		RuleFor(x => x.TotalCost)
			.GreaterThan(0)
			.When(x => x.TotalCost.HasValue);

		RuleFor(x => x.Notes)
			.MaximumLength(500)
			.When(x => x.Notes != null);
	}
}
