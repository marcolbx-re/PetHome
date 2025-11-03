using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PetHome.Application.Core;
using PetHome.Application.Interfaces;
using PetHome.Domain;
using PetHome.Persistence;
using PetHome.Persistence.Models;

namespace PetHome.Application.Accounts.RegisterAsOwner;

public class RegisterAsOwnerCommand
{
	public record RegisterOwnerCommandRequest(RegisterAsOwnerRequest OwnerCreateRequest, string CallbackUrl)
		: IRequest<Result<Guid>>, ICommandBase;
	
	internal class RegisterOwnerCommandHandler
		: IRequestHandler<RegisterOwnerCommandRequest, Result<Guid>>
	{
		private readonly PetHomeDbContext _context;
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailService _emailService;

		public RegisterOwnerCommandHandler(
			PetHomeDbContext context,
			UserManager<AppUser> userManager,
			IEmailService emailService
		)
		{
			_context = context;
			_userManager = userManager;
			_emailService = emailService;
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
			
			if (result2)
			{
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				var link = $"{request.CallbackUrl}?userId={user.Id}&token={Uri.EscapeDataString(token)}";
				await _emailService.SendEmailAsync(
					dto.Email!,
					"Welcome!",
					"Welcome",
					new { FullName = dto.FirstName, SetPasswordUrl = link });
			}
			
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
