using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PetHome.Application.Core;
using PetHome.Domain;
using PetHome.Persistence;
using PetHome.Persistence.Models;

namespace PetHome.Application.Accounts.RegisterAsOwner;

public class RegisterAsOwnerCommand
{
	public record RegisterOwnerCommandRequest(RegisterAsOwnerRequest OwnerCreateRequest)
		: IRequest<Result<Guid>>, ICommandBase;
	
	internal class RegisterOwnerCommandHandler
		: IRequestHandler<RegisterOwnerCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public RegisterOwnerCommandHandler(
			PetHomeDbContext context,
			UserManager<AppUser> userManager
		)
		{
			_context = context;
			_userManager = userManager;
		}
        
		public async Task<Result<Guid>> Handle(
			RegisterOwnerCommandRequest request, 
			CancellationToken cancellationToken
		)
		{
			var dto = request.OwnerCreateRequest;
			var user = new AppUser
			{
				UserName = dto.Email,
				Email = dto.Email,
				FullName = dto.FirstName + dto.LastName,
			};

			var result = await _userManager.CreateAsync(user, dto.Password!);
			if (!result.Succeeded)
				return Result<Guid>.Failure("No se pudo crear el usuario");

			await _userManager.AddToRoleAsync(user, CustomRoles.OWNER);

            
			var owner = new Domain.Owner(dto.FirstName, dto.LastName, dto.Email,
				dto.PhoneNumber, dto.IsNewsletterSubscribed, dto.IdentificationType, dto.IdentificationNumber, user);
			_context.Add(owner);

			var result2 = await _context.SaveChangesAsync(cancellationToken) > 0;
			return result2 
				? Result<Guid>.Success(owner.Id)
				: Result<Guid>.Failure("No se pudo insertar el Owner");
		}
	}
    
	public class RegisterOwnerCommandRequestValidator
		: AbstractValidator<RegisterOwnerCommandRequest>
	{
		public RegisterOwnerCommandRequestValidator()
		{
			RuleFor(x => x.OwnerCreateRequest).SetValidator(new RegisterAsOwnerValidator());
		}

	}
}
