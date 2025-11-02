using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
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
                
                var ownerClient = new AppUser
                {
                    FullName = "Apellido",
                    UserName = "Nombre",
                    Email = "nombre.apellido@gmail.com"
                };
                
                await userManager.CreateAsync(ownerClient, "Password123$");
                await userManager.AddToRoleAsync(ownerClient, CustomRoles.OWNER);
            }
            await context.SaveChangesAsync();

        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<PetHomeDbContext>();
            logger.LogError(e.Message);
        }
    }
    
    public static async Task SeedOwnersAsync(
        this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();
        try
        {
            var dbContext = service.GetRequiredService<PetHomeDbContext>();
            if (dbContext.Owners is null || dbContext.Owners.Any()) return;
            var jsonString = GetJsonFile("owners.json");

            if (jsonString is null) return;

            var owners = JsonConvert.DeserializeObject<List<Owner>>(jsonString);

            if (owners is null || owners.Any() == false) return;

            dbContext.Owners.AddRange(owners!);
            await dbContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<PetHomeDbContext>();
            logger.LogError(ex.Message);
        }
    }
    
    public static async Task SeedPetsAsync(
        this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();
        try
        {
            var dbContext = service.GetRequiredService<PetHomeDbContext>();
            if (dbContext.Pets is null || dbContext.Pets.Any()) return;
            var jsonString = GetJsonFile("pets.json");

            if (jsonString is null) return;

            var pets = JsonConvert.DeserializeObject<List<Pet>>(jsonString);

            if (pets is null || !pets.Any()) return;

            dbContext.Pets.AddRange(pets!);
            await dbContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<PetHomeDbContext>();
            logger.LogError(ex.Message);
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