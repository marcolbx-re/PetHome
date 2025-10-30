using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Persistence;
using ValidationException = FluentValidation.ValidationException;

namespace PetHome.Application.Pets.PatchPet;

public class PetPatchCommand
{
	public record PetPatchCommandRequest(
		Guid? Id, JsonPatchDocument<PetPatchRequest> Patch
	): IRequest<Result<Guid>>, ICommandBase;

	internal class PetPatchCommandHandler
		: IRequestHandler<PetPatchCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;
		private readonly IMapper _mapper;
		private readonly IValidator<PetPatchRequest> _validator;

		public PetPatchCommandHandler(PetHomeDbContext context, IMapper mapper, IValidator<PetPatchRequest> validator)
		{
			_context = context;
			_mapper = mapper;
			_validator = validator;
		}

		public async Task<Result<Guid>> Handle(
			PetPatchCommandRequest request, 
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

			// Map entity to DTO
			var petToPatch = _mapper.Map<PetPatchRequest>(pet);

			// Apply JSON Patch
			request.Patch.ApplyTo(petToPatch);

			// Map DTO back to entity
			_mapper.Map(petToPatch, pet);
			
			// var validationResult = await _validator.ValidateAsync(petToPatch, cancellationToken);
			// if (!validationResult.IsValid)
			// 	throw new ValidationException(validationResult.Errors);
			
			//_context.Entry(pet).State = EntityState.Modified;
			var savedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;

			return savedSuccess 
				? Result<Guid>.Success(pet.Id)
				: Result<Guid>.Failure("Errores en el update de Pet");

		}
	}
	
	// public class PetPatchCommandRequestValidator
	// 	: AbstractValidator<PetPatchCommandRequest>
	// {
	// 	public PetPatchCommandRequestValidator()
	// 	{
	// 		RuleFor(x => x.Patch).SetValidator(new PetPatchValidator());
	// 	}
	// }

}
