using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Persistence.Test;

namespace PetHome.Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistence(
		this IServiceCollection services,
		IConfiguration configuration
	)
	{
		services.AddDbContext<PetHomeDbContext>(opt => {
			opt.LogTo(Console.WriteLine, new [] {
				DbLoggerCategory.Database.Command.Name
			}, LogLevel.Information).EnableSensitiveDataLogging();

			opt.UseSqlite(configuration.GetConnectionString("SqliteDatabase"))
				.UseAsyncSeeding(async (context, status, cancellationToken) =>
				{
					var dbContext = (PetHomeDbContext)context;
					var logger = context.GetService<ILogger<PetHomeDbContext>>();
					try
					{
						await DbSeeder.SeedPersonsAsync(dbContext, logger, cancellationToken);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						throw;
					}
				});
		});

		return services;
	}
}
