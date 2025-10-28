using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Application.Interfaces;
using PetHome.Persistence.Models;

namespace PetHome.Application.Accounts.Login;

public class LoginCommand
{
	public record LoginCommandRequest(LoginRequest LoginRequest)
		: IRequest<Result<Profile>>;


	internal class LoginCommandHandler
		: IRequestHandler<LoginCommandRequest, Result<Profile>>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;

		public LoginCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService)
		{
			_userManager = userManager;
			_tokenService = tokenService;
		}

		public async Task<Result<Profile>> Handle(
			LoginCommandRequest request,
			CancellationToken cancellationToken
		)
		{
			var user = await _userManager.Users
				.FirstOrDefaultAsync(x => x.Email == request.LoginRequest.Email, cancellationToken: cancellationToken);

			if (user is null)
			{
				return Result<Profile>.Failure("No se encontro el usuario");
			}

			var result = await _userManager
				.CheckPasswordAsync(user, request.LoginRequest.Password!);

			if(!result)
			{
				return Result<Profile>.Failure("Las credenciales son incorrectas");
			}

			var profile = new Profile
			{
				Email = user.Email,
				Username = user.UserName,
				Token = await _tokenService.CreateToken(user),
				FullName = user.FullName
			};

			return Result<Profile>.Success(profile);
		}
	}
	
	public class LoginCommandRequestValidator : AbstractValidator<LoginCommandRequest>
	{
		public LoginCommandRequestValidator()
		{
			RuleFor(x => x.LoginRequest).SetValidator (new LoginValidator());
		}
	}
}