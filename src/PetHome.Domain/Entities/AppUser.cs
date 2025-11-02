using Microsoft.AspNetCore.Identity;
using PetHome.Domain;

namespace PetHome.Persistence.Models;

public class AppUser : IdentityUser
{
	public string? FullName { get; set; }
	public Owner? Owner { get; set; }
}
