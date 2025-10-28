using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Persistence;

namespace PetHome.Application.Pets.UpdatePet;

public class PetUpdateCommand
{
	public record PetUpdateCommandRequest(
		PetUpdateRequest PetUpdateRequest, 
		Guid? Id
	): IRequest<Result<Guid>>, ICommandBase;

	internal class PetUpdateCommandHandler
		: IRequestHandler<PetUpdateCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;

		public PetUpdateCommandHandler(PetHomeDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Guid>> Handle(
			PetUpdateCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var id = request.Id;

			var pet = await _context.Pets!
				.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            
			if (pet is null)
			{
				return Result<Guid>.Failure("El pet no existe");
			}

			var updateRequest = request.PetUpdateRequest;
			pet.BirthDate = (DateTime)updateRequest.BirthDate!;
			pet.OwnerId = updateRequest.OwnerId;
			pet.IsDeclawed = updateRequest.IsDeclawed;
			pet.Gender = updateRequest.Gender;
			pet.Name = updateRequest.Name;
			pet.Type = updateRequest.Type;
			pet.Size = updateRequest.Size;
			pet.Breed = updateRequest.Breed;
			pet.RequiresSpecialDiet = updateRequest.RequiresSpecialDiet;
			pet.SpecialInstructions = updateRequest.SpecialInstructions;
			pet.Photos =  updateRequest.Photos;

			_context.Entry(pet).State = EntityState.Modified;
			var savedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;

			return savedSuccess 
				? Result<Guid>.Success(pet.Id)
				: Result<Guid>.Failure("Errores en el update de Pet");

		}
	}
	
	public class PetUpdateCommandRequestValidator
		: AbstractValidator<PetUpdateCommandRequest>
	{
		public PetUpdateCommandRequestValidator()
		{
			RuleFor(x => x.PetUpdateRequest).SetValidator(new PetUpdateValidator());
			RuleFor(x => x.Id).NotNull();
		}
	}

}
