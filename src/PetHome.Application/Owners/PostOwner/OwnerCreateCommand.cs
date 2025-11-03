using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PetHome.Application.Core;
using PetHome.Application.Interfaces;
using PetHome.Domain;
using PetHome.Persistence;
using PetHome.Persistence.Models;

namespace PetHome.Application.Owner.OwnerCreate;

public class OwnerCreateCommand
{
	public record OwnerCreateCommandRequest(OwnerCreateRequest OwnerCreateRequest)
		: IRequest<Result<Guid>>, ICommandBase;
	
	internal class OwnerCreateCommandHandler
    : IRequestHandler<OwnerCreateCommandRequest, Result<Guid>>
    {
        private readonly PetHomeDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public OwnerCreateCommandHandler(
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
            OwnerCreateCommandRequest request, 
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

            var userResult = await _userManager.CreateAsync(user);
            if (!userResult.Succeeded)
                return Result<Guid>.Failure("No se pudo crear el usuario");

            await _userManager.AddToRoleAsync(user, CustomRoles.OWNER);
            
            var owner = new Domain.Owner(dto.FirstName!, dto.LastName!, dto.Email!,
                dto.PhoneNumber!, dto.IsNewsletterSubscribed, dto.IdentificationType, dto.IdentificationNumber!, user);
            _context.Add(owner);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
            {
                // var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //
                // // Combine token into CallbackUrl if not already included
                // var link = $"{request.CallbackUrl}?userId={user.Id}&token={Uri.EscapeDataString(token)}";
                // var callbackUrl = Url.Action(
                //     action: "SetPassword", 
                //     controller: "Account",
                //     values: new { userId = user.Id, token = token },
                //     protocol: Request.Scheme);
                // await _emailService.SendEmailAsync(
                //     dto.Email!,
                //     "Welcome!",
                //     "Welcome",
                //     new { FullName = dto.FirstName, SetPasswordUrl = request.Link });
            }
            
            return result 
                        ? Result<Guid>.Success(owner.Id)
                        : Result<Guid>.Failure("No se pudo insertar el Owner");
        }
    }
    
    public class OwnerCreateCommandRequestValidator
        : AbstractValidator<OwnerCreateCommandRequest>
    {
        public OwnerCreateCommandRequestValidator()
        {
            RuleFor(x => x.OwnerCreateRequest).SetValidator(new OwnerCreateValidator());
        }

    }
}
