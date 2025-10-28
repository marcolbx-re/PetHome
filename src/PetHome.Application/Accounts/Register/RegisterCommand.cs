using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Application.Interfaces;
using PetHome.Persistence.Models;

namespace PetHome.Application.Accounts.Register;

public class RegisterCommand
{
	public record RegisterCommandRequest(RegisterRequest RegisterRequest)
    : IRequest<Result<Profile>>, ICommandBase;


    internal class RegisterCommandHandler
    : IRequestHandler<RegisterCommandRequest, Result<Profile>>
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(
            UserManager<AppUser> userManager, 
            ITokenService tokenService
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<Profile>> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
           
            if(await  _userManager.Users
            .AnyAsync(x=> x.Email == request.RegisterRequest.Email, cancellationToken: cancellationToken))
            {
                Result<Profile>.Failure("El email ya fue registrado por otro usuario");
            }

            if(await _userManager.Users
            .AnyAsync(x=>x.UserName == request.RegisterRequest.Username, cancellationToken: cancellationToken))
            {
                Result<Profile>.Failure("El username ya fue registrado");
            }

            AppUser user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.RegisterRequest.Email,
                UserName = request.RegisterRequest.Username,
                FullName = request.RegisterRequest.FullName
            };
           
            var result =  await _userManager.CreateAsync(user, request.RegisterRequest.Password!);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Client");
                
                var profile = new Profile
                {
                    Email = user.Email,
                    Token = await _tokenService.CreateToken(user),
                    Username = user.UserName
                };

                return Result<Profile>.Success(profile);
            }
            return Result<Profile>.Failure("Errores en el registro del usuario");
        }
    }

    public class RegiterCommandRequestValidator : AbstractValidator<RegisterCommandRequest>
    {
        public RegiterCommandRequestValidator()
        {
            RuleFor(x => x.RegisterRequest).SetValidator(new RegisterValidator());
        }
    }
}