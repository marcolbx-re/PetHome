// using Microsoft.Extensions.Logging;
// using PetHome.Domain;
// using Newtonsoft.Json;
//
// namespace PetHome.Persistence;
//
// public static class SeedDatabase
// {
// 	public static async Task SeedPetsAsync(
// 		PetHomeDbContext dbContext,
// 		ILogger? logger,
// 		CancellationToken cancellationToken)
// 	{
// 		try
// 		{
// 			if (dbContext.Pets is null || dbContext.Pets.Any()) return;
// 			var jsonString = GetJsonFile("pets.json");
//
// 			if (jsonString is null) return;
//
// 			var pets = JsonConvert.DeserializeObject<List<Pet>>(jsonString);
//
// 			if (pets is null || pets.Any() == false) return;
//
// 			dbContext.Pets.AddRange(pets!);
// 			await dbContext.SaveChangesAsync(cancellationToken);
//
// 		}
// 		catch (Exception ex)
// 		{
// 			logger?.LogWarning(ex, "Fallo cargando la data de instructores");
// 		}
// 	}
// 	
// 	public static async Task SeedOwnersAsync(
// 		PetHomeDbContext dbContext,
// 		ILogger? logger,
// 		CancellationToken cancellationToken)
// 	{
// 		try
// 		{
// 			if (dbContext.Owners is null || dbContext.Owners.Any()) return;
// 			var jsonString = GetJsonFile("owners.json");
//
// 			if (jsonString is null) return;
//
// 			var owners = JsonConvert.DeserializeObject<List<Owner>>(jsonString);
//
// 			if (owners is null || owners.Any() == false) return;
//
// 			dbContext.Owners.AddRange(owners!);
// 			await dbContext.SaveChangesAsync(cancellationToken);
//
// 		}
// 		catch (Exception ex)
// 		{
// 			logger?.LogWarning(ex, "Fallo cargando la data de instructores");
// 		}
// 	}
// 	
// 	public static async Task SeedStaysAsync(
// 		PetHomeDbContext dbContext,
// 		ILogger? logger,
// 		CancellationToken cancellationToken)
// 	{
// 		try
// 		{
// 			if (dbContext.Owners is null || dbContext.Owners.Any()) return;
// 			var jsonString = GetJsonFile("owners.json");
//
// 			if (jsonString is null) return;
//
// 			var owners = JsonConvert.DeserializeObject<List<Owner>>(jsonString);
//
// 			if (owners is null || owners.Any() == false) return;
//
// 			dbContext.Owners.AddRange(owners!);
// 			await dbContext.SaveChangesAsync(cancellationToken);
//
// 		}
// 		catch (Exception ex)
// 		{
// 			logger?.LogWarning(ex, "Fallo cargando la data de instructores");
// 		}
// 	}
// 	
// 	private static string GetJsonFile(string fileName)
// 	{
// 		var leerForma1 = Path.Combine(
// 			Directory.GetCurrentDirectory(),
// 			"src",
// 			"PetHome.Persistence",
// 			"SeedData",
// 			fileName
// 		);
//
// 		var leerForma2 = Path.Combine(
// 			Directory.GetCurrentDirectory(),
// 			"SeedData",
// 			fileName
// 		);
//
// 		var leerForma3 = Path.Combine(
// 			AppContext.BaseDirectory,
// 			"SeedData",
// 			fileName
// 		);
//
// 		if (File.Exists(leerForma1)) return File.ReadAllText(leerForma1);
// 		if (File.Exists(leerForma2)) return File.ReadAllText(leerForma2);
// 		if (File.Exists(leerForma3)) return File.ReadAllText(leerForma3);
//
// 		return null!;
// 	}
// }
