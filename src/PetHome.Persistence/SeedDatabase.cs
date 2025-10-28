using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetHome.Domain;
using Newtonsoft.Json;
using PetHome.Persistence.Models;

namespace PetHome.Persistence;

public static class SeedDatabase
{
	public static async Task SeedRolesAndUsersAsync(
     UserManager<AppUser> userManager,
     RoleManager<IdentityRole> roleManager,
     ILogger? logger,
     CancellationToken cancellationToken
    )
    {
        try
        {
            if (userManager.Users.Any()) return;

            var adminId = "d3b07384-d9a0-4c9b-8a0d-1b2e2b3c4d5e";
            var clientId = "e4b07384-d9a0-4c9b-8a0d-1b2e2b3c4d5f";

            var roleAdmin = new IdentityRole
            {
                Id = adminId,
                Name = CustomRoles.ADMIN,
                NormalizedName = CustomRoles.ADMIN.ToUpperInvariant()
            };

            var roleClient = new IdentityRole
            {
                Id = clientId,
                Name = CustomRoles.CLIENT,
                NormalizedName = CustomRoles.CLIENT.ToUpperInvariant()
            };

            if (!await roleManager.RoleExistsAsync(CustomRoles.ADMIN))
            {
                await roleManager.CreateAsync(roleAdmin);
            }

            if (!await roleManager.RoleExistsAsync(CustomRoles.CLIENT))
            {
                await roleManager.CreateAsync(roleClient);
            }

            var userAdmin = new AppUser
            {
                UserName = "mlbx",
                Email = "mlbx@gmail.com"
            };

            await userManager.CreateAsync(userAdmin, "Password123$");

            var userClient = new AppUser
            {
                UserName = "juanperez",
                Email = "juan.perez@gmail.com"
            };

            await userManager.CreateAsync(userClient, "Password123$");

            // agregar un determinado role a cada usuario
            await userManager.AddToRoleAsync(userAdmin, CustomRoles.ADMIN);
            await userManager.AddToRoleAsync(userClient, CustomRoles.CLIENT);


            await roleManager
            .AddClaimAsync(
                roleAdmin,
                new Claim(CustomClaims.POLICIES, PolicyMaster.OWNER_READ)
            );

            await roleManager
            .AddClaimAsync(
                roleAdmin,
                new Claim(CustomClaims.POLICIES, PolicyMaster.OWNER_UPDATE)
            );

            await roleManager
            .AddClaimAsync(
                roleAdmin,
                new Claim(CustomClaims.POLICIES, PolicyMaster.OWNER_CREATE)
            );

            await roleManager
           .AddClaimAsync(
               roleAdmin,
               new Claim(CustomClaims.POLICIES, PolicyMaster.OWNER_DELETE)
           );

            await roleManager
           .AddClaimAsync(
               roleAdmin,
               new Claim(CustomClaims.POLICIES, PolicyMaster.PET_READ)
           );

            await roleManager
           .AddClaimAsync(
               roleAdmin,
               new Claim(CustomClaims.POLICIES, PolicyMaster.PET_UPDATE)
           );

            await roleManager
           .AddClaimAsync(
               roleAdmin,
               new Claim(CustomClaims.POLICIES, PolicyMaster.PET_CREATE)
           );

            await roleManager
            .AddClaimAsync(
                roleClient,
                new Claim(CustomClaims.POLICIES, PolicyMaster.PET_READ)
            );

            await roleManager
            .AddClaimAsync(
                roleClient,
                new Claim(CustomClaims.POLICIES, PolicyMaster.OWNER_READ)
            );
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "Fallo el proceso de identity seed");
        }

    }
	public static async Task SeedPetsAsync(
		PetHomeDbContext dbContext,
		ILogger? logger,
		CancellationToken cancellationToken)
	{
		try
		{
			if (dbContext.Pets is null || dbContext.Pets.Any()) return;
			var jsonString = GetJsonFile("pets.json");

			if (jsonString is null) return;

			var pets = JsonConvert.DeserializeObject<List<Pet>>(jsonString);

			if (pets is null || pets.Any() == false) return;

			dbContext.Pets.AddRange(pets!);
			await dbContext.SaveChangesAsync(cancellationToken);

		}
		catch (Exception ex)
		{
			logger?.LogWarning(ex, "Fallo cargando la data de instructores");
		}
	}
	
	public static async Task SeedOwnersAsync(
		PetHomeDbContext dbContext,
		ILogger? logger,
		CancellationToken cancellationToken)
	{
		try
		{
			if (dbContext.Owners is null || dbContext.Owners.Any()) return;
			var jsonString = GetJsonFile("owners.json");

			if (jsonString is null) return;

			var owners = JsonConvert.DeserializeObject<List<Owner>>(jsonString);

			if (owners is null || owners.Any() == false) return;

			dbContext.Owners.AddRange(owners!);
			await dbContext.SaveChangesAsync(cancellationToken);

		}
		catch (Exception ex)
		{
			logger?.LogWarning(ex, "Fallo cargando la data de instructores");
		}
	}
	
	public static async Task SeedStaysAsync(
		PetHomeDbContext dbContext,
		ILogger? logger,
		CancellationToken cancellationToken)
	{
		try
		{
			if (dbContext.Owners is null || dbContext.Owners.Any()) return;
			var jsonString = GetJsonFile("owners.json");

			if (jsonString is null) return;

			var owners = JsonConvert.DeserializeObject<List<Owner>>(jsonString);

			if (owners is null || owners.Any() == false) return;

			dbContext.Owners.AddRange(owners!);
			await dbContext.SaveChangesAsync(cancellationToken);

		}
		catch (Exception ex)
		{
			logger?.LogWarning(ex, "Fallo cargando la data de instructores");
		}
	}
	
	private static string GetJsonFile(string fileName)
	{
		var leerForma1 = Path.Combine(
			Directory.GetCurrentDirectory(),
			"src",
			"PetHome.Persistence",
			"SeedData",
			fileName
		);

		var leerForma2 = Path.Combine(
			Directory.GetCurrentDirectory(),
			"SeedData",
			fileName
		);

		var leerForma3 = Path.Combine(
			AppContext.BaseDirectory,
			"SeedData",
			fileName
		);

		if (File.Exists(leerForma1)) return File.ReadAllText(leerForma1);
		if (File.Exists(leerForma2)) return File.ReadAllText(leerForma2);
		if (File.Exists(leerForma3)) return File.ReadAllText(leerForma3);

		return null!;
	}
}
