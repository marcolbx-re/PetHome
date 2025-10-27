// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using PetHome.Persistence;
// using PetHome.Persistence.Test;
//
// var services = new ServiceCollection();
// services.AddLogging(l =>
// {
// 	l.ClearProviders();
// });
//
// //services.AddDbContext<PetHomeDbContext>();
// services.AddDbContext<PetHomeDbContext>(options => options.UseSqlite("Data Source=LocalDatabase.db"));
// var provider =  services.BuildServiceProvider();
//
// try
// {
// 	using var scope = provider.CreateScope();
// 	var context = scope.ServiceProvider.GetRequiredService<PetHomeDbContext>();
// 	await DbSeeder.SeedPersonsAsync(context);
// 	await DbSeeder.SeedOwnersAsync(context);
// 	await DbSeeder.SeedPetsAsync(context);
//
// 	foreach (var person in context.Persons)
// 	{
// 		Console.WriteLine($"{person.Id}: {person.FirstName} {person.LastName}, Age {person.Age}");
// 	}
// 	
// 	Console.WriteLine("Migration successful");
// }
// catch (Exception e)
// {
// 	Console.WriteLine(e);
// 	throw;
// }
