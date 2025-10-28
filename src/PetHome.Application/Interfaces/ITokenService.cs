using PetHome.Persistence.Models;

namespace PetHome.Application.Interfaces;

public interface ITokenService
{
	Task<string> CreateToken(AppUser user);
}
