using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetHome.Domain;
using PetHome.Persistence;
using PetHome.Persistence.Models;

namespace PetHome.WebApi.Extensions;

public static class DataSeed
{
    public static async Task SeedDataAuthentication(
        this IApplicationBuilder app
    )
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();
        try
        {
            var context = service.GetRequiredService<PetHomeDbContext>();
            var userManager = service.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any())
            {
                var userAdmin = new AppUser
                {
                    FullName = "Marco Bethke",
                    UserName = "marbet",
                    Email = "admin@gmail.com"
                };

                await userManager.CreateAsync(userAdmin, "Password123$");
                await userManager.AddToRoleAsync(userAdmin, CustomRoles.ADMIN);

                var userClient = new AppUser
                {
                    FullName = "Juan Perez",
                    UserName = "juanperez",
                    Email = "juan.perez@gmail.com"
                };

                await userManager.CreateAsync(userClient, "Password123$");
                await userManager.AddToRoleAsync(userClient, CustomRoles.CLIENT);
            }
            await context.SaveChangesAsync();

        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<PetHomeDbContext>();
            logger.LogError(e.Message);
        }
    }
}