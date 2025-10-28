using Microsoft.EntityFrameworkCore;
using PetHome.Domain;

namespace PetHome.Persistence.Test;

public static class DbSeeder
{
	public static async Task SeedOwnersAsync(PetHomeDbContext context)
	{
		await context.Database.MigrateAsync(); // Apply migrations first

		if (context.Owners.Any())
			return;

		var owners = new List<Owner>
		{
			new Owner(
				firstName: "Alice",
				lastName: "Johnson",
				email: "alice@example.com",
				phoneNumber: "123-456-7890",
				isNewsletterSubscribed: true,
				IdentificationType.DNI,
				"MyDNI"
			),
			new Owner(
				firstName: "Bob",
				lastName: "Smith",
				email: "bob@example.com",
				phoneNumber: "987-654-3210",
				isNewsletterSubscribed: false,
				IdentificationType.NIE,
				"MyNIE"
			)
		};

		await context.Owners.AddRangeAsync(owners);
		await context.SaveChangesAsync();
	}

	public static async Task SeedPetsAsync(PetHomeDbContext context)
	{
		await context.Database.MigrateAsync();

		// Skip if pets already exist
		if (context.Pets.Any())
			return;

		// Get all owners
		var owners = await context.Owners.ToListAsync();

		var pets = new List<Pet>();

		foreach (var owner in owners)
		{

			// Example pet 2
			var pet2 = new Pet(
				name: "Whiskers",
				breed: "Siamese",
				birthDate: new DateTime(2019, 3, 20),
				ownerId: owner.Id,
				specialInstructions: "Allergic to certain foods",
				gender: GenderType.Female,
				requiresSpecialDiet: true,
				isDeclawed: false,
				petType: PetType.Cat,
				size: Size.Small
			);

			pets.Add(pet2);
		}

		await context.Pets.AddRangeAsync(pets);
		await context.SaveChangesAsync();
	}
}