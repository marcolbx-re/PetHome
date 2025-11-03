using PetHome.Persistence;
using PetHome.Persistence.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PetHome.Application.Interfaces;
using PetHome.Infrastructure.Security;

namespace PetHome.WebApi.Extensions;

public static class IdentityServiceExtensions
{
	public static IServiceCollection AddIdentityServices(
		this IServiceCollection services,
		IConfiguration configuration
	)
	{
		services.AddIdentityCore<AppUser>(opt =>
		{
			opt.Password.RequireNonAlphanumeric = false;
			opt.User.RequireUniqueEmail = true;
		}).AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<PetHomeDbContext>()
			.AddDefaultTokenProviders();


		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IUserAccessor, UserAccessor>();
		
		var key = 
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]!));
		
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(opt => {
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = key,
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
		return services;
	}
}