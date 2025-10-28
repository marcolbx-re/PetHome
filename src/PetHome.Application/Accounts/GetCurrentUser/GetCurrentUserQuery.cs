using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Core;
using PetHome.Application.Interfaces;
using PetHome.Persistence.Models;

namespace PetHome.Application.Accounts.GetCurrentUser;

public class GetCurrentUserQuery
{
	public record GetCurrentUserQueryRequest(GetCurrentUserRequest getCurrentUserRequest)
		: IRequest<Result<Profile>>;

	internal class GetCurrentUserQueryHandler :
		IRequestHandler<GetCurrentUserQueryRequest, Result<Profile>>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;

		public GetCurrentUserQueryHandler(
			UserManager<AppUser> userManager, 
			ITokenService tokenService
		)
		{
			_userManager = userManager;
			_tokenService = tokenService;
		}

		public async Task<Result<Profile>> Handle(
			GetCurrentUserQueryRequest request, 
			CancellationToken cancellationToken
		)
		{
			AppUser? user = await _userManager.Users
				.FirstOrDefaultAsync(x => x.Email == request.getCurrentUserRequest.Email, cancellationToken: cancellationToken);

			if(user is null)
			{
				return Result<Profile>.Failure("No se encontro el usuario");
			}

			var profile = new Profile
			{
				Email = user.Email,
				Token = await _tokenService.CreateToken(user),
				Username = user.UserName,
				FullName = user.FullName
			};

			return Result<Profile>.Success(profile);
		}
	}


}
