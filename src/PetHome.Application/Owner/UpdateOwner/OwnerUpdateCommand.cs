using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Persistence;

namespace PetHome.Application.Owner.UpdateOwner;

public class OwnerUpdateCommand
{
	public record OwnerUpdateCommandRequest(
		OwnerUpdateRequest OwnerUpdateRequest, 
		Guid? Id
	): IRequest<Result<Guid>>, ICommandBase;

	internal class OwnerUpdateCommandHandler
		: IRequestHandler<OwnerUpdateCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;

		public OwnerUpdateCommandHandler(PetHomeDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Guid>> Handle(
			OwnerUpdateCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var id = request.Id;

			var owner = await _context.Owners!
				.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            
			if (owner is null)
			{
				return Result<Guid>.Failure("El Owner no existe");
			}

			owner.Email = request.OwnerUpdateRequest.Email;
			owner.FirstName = request.OwnerUpdateRequest.FirstName;
			owner.LastName = request.OwnerUpdateRequest.LastName;
			owner.IdentificationNumber = request.OwnerUpdateRequest.IdentificationNumber;
			owner.IdentificationType = request.OwnerUpdateRequest.IdentificationType;
			owner.PhoneNumber = request.OwnerUpdateRequest.PhoneNumber;
			owner.IsNewsletterSubscribed = request.OwnerUpdateRequest.IsNewsletterSubscribed;

			_context.Entry(owner).State = EntityState.Modified;
			var savedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;

			return savedSuccess 
				? Result<Guid>.Success(owner.Id)
				: Result<Guid>.Failure("Errores en el update de Owner");

		}
	}
	
	public class OwnerUpdateCommandRequestValidator
		: AbstractValidator<OwnerUpdateCommandRequest>
	{
		public OwnerUpdateCommandRequestValidator()
		{
			RuleFor(x => x.OwnerUpdateRequest).SetValidator(new OwnerUpdateValidator());
			RuleFor(x => x.Id).NotNull();
		}
	}

}
