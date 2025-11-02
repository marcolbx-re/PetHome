using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Persistence;
using ValidationException = FluentValidation.ValidationException;

namespace PetHome.Application.Stays.BackOffice.PatchStay;

public class StayPatchCommand
{
	public record StayPatchCommandRequest(
		Guid? Id, JsonPatchDocument<StayPatchRequest> Patch
	): IRequest<Result<Guid>>, ICommandBase;

	internal class StayPatchCommandHandler
		: IRequestHandler<StayPatchCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;
		private readonly IMapper _mapper;
		private readonly IValidator<StayPatchRequest> _validator;

		public StayPatchCommandHandler(PetHomeDbContext context, IMapper mapper, IValidator<StayPatchRequest> validator)
		{
			_context = context;
			_mapper = mapper;
			_validator = validator;
		}

		public async Task<Result<Guid>> Handle(
			StayPatchCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var id = request.Id;

			var stay = await _context.Stays!
				.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            
			if (stay is null)
			{
				return Result<Guid>.Failure("El pet no existe");
			}

			// Map entity to DTO
			var stayToPatch = _mapper.Map<StayPatchRequest>(stay);

			// Apply JSON Patch
			request.Patch.ApplyTo(stayToPatch);

			// Map DTO back to entity
			_mapper.Map(stayToPatch, stay);
			
			var validationResult = await _validator.ValidateAsync(stayToPatch, cancellationToken);
			if (!validationResult.IsValid)
				throw new ValidationException(validationResult.Errors);
			
			var savedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;

			return savedSuccess 
				? Result<Guid>.Success(stay.Id)
				: Result<Guid>.Failure("Errores en el update de Stay");

		}
	}
	
	public class StayPatchCommandRequestValidator
		: AbstractValidator<StayPatchRequest>
	{
		public StayPatchCommandRequestValidator()
		{
			RuleFor(x => x).SetValidator(new StayPatchValidator());
		}
	}
}
