using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PetHome.Domain;

namespace PetHome.Persistence;

public class PetHomeDbContext : DbContext
{
	public PetHomeDbContext()
	{
	}
	public PetHomeDbContext(DbContextOptions<PetHomeDbContext> options) : base(options)
	{
	}
	
	public DbSet<Owner>? Owners {get;set;}
	public DbSet<Pet>? Pets {get;set;}
	public DbSet<Transaction>? Transactions {get;set;}
	public DbSet<Stay>? Stays {get;set;}
	public DbSet<CareActivity>? Activities {get;set;}
	public DbSet<Photo>? Photos {get;set;}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite("Data Source=LocalDatabase.db")
			.LogTo(Console.WriteLine, new []{DbLoggerCategory.Database.Command.Name},
				Microsoft.Extensions.Logging.LogLevel.Information).EnableSensitiveDataLogging();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) //TODO agregar limite para int, VARCHAR 
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<Owner>().ToTable("owners");
		modelBuilder.Entity<Stay>().ToTable("stays");
		modelBuilder.Entity<Photo>().ToTable("photos");
		
		modelBuilder.Entity<Owner>()
			.HasMany(o => o.Pets)
			.WithOne(p => p.Owner)
			.HasForeignKey(p => p.OwnerId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Pet>()
			.HasMany(p => p.Stays)
			.WithOne(s => s.Pet)
			.HasForeignKey(s => s.PetId)
			.OnDelete(DeleteBehavior.Restrict);
		
		modelBuilder.Entity<Pet>()
			.HasMany(m => m.Photos)
			.WithOne(m => m.Pet)
			.HasForeignKey(m => m.PetId)
			.OnDelete(DeleteBehavior.Cascade);
		
		modelBuilder.Entity<Stay>()
			.HasMany(s => s.CareActivities)
			.WithOne(a => a.Stay)
			.HasForeignKey(a => a.StayId)
			.OnDelete(DeleteBehavior.Cascade);
		
		modelBuilder.Entity<Stay>()
			.HasOne(s => s.Transaction)
			.WithOne(p => p.Stay)
			.HasForeignKey<Transaction>(p => p.StayId)
			.OnDelete(DeleteBehavior.Restrict);
		
		modelBuilder.Entity<Transaction>()
			.Property(t => t.Metadata)
			.HasConversion(
				v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
				v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null)!);
		
		modelBuilder.Entity<Pet>()
			.HasDiscriminator<PetType>(nameof(Pet.Type))
			.HasValue<Dog>(PetType.Dog)
			.HasValue<Cat>(PetType.Cat);
	}
}
