using Microsoft.AspNetCore.Identity;

namespace PetHome.Persistence.Models;

public class AppUser : IdentityUser
{
	public string? FullName { get; set; }
}
