using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Domain;

namespace PetHome.Persistence.Test;

public static class DbSeeder
{
	public static async Task SeedPersonsAsync(PetHomeDbContext context,
		ILogger logger, CancellationToken ct)
	{
		try
		{
// Apply pending migrations
			await context.Database.MigrateAsync();

			Console.WriteLine("S111111111111111");
			// If table already has data, skip
			if (context.Persons.Any())
				return;

			var random = new Random();
			var people = new List<Person>();

			for (int i = 0; i < 10; i++)
			{
				people.Add(new Person
				{
					FirstName = "First" + i,
					LastName = "Last" + i,
					Age = random.Next(18, 60)
				});
			}

			await context.Persons.AddRangeAsync(people);
			await context.SaveChangesAsync();
			Console.WriteLine("SUCCESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
		}
		catch (Exception e)
		{
			logger?.LogWarning(e, "Fallo cargando la data de Persons");
			Console.WriteLine(e);
			throw;
		}
	}
	
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
			// Example pet 1
			var pet1 = new Dog(
				name: "Fido",
				breed: "Labrador",
				birthDate: new DateTime(2020, 5, 10),
				ownerId: owner.Id,
				specialInstructions: "No chocolate",
				gender: GenderType.Male,
				requiresSpecialDiet: true,
				size: DogSize.Medium,
				requiresExtraExercise: false
			);

			// Example pet 2
			var pet2 = new Cat(
				name: "Whiskers",
				breed: "Siamese",
				birthDate: new DateTime(2019, 3, 20),
				ownerId: owner.Id,
				specialInstructions: "Allergic to certain foods",
				gender: GenderType.Female,
				requiresSpecialDiet: true,
				isDeclawed: false
			);

			pets.Add(pet1);
			pets.Add(pet2);
		}

		await context.Pets.AddRangeAsync(pets);
		await context.SaveChangesAsync();
	}
}