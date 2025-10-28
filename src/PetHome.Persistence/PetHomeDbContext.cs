using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetHome.Domain;
using PetHome.Persistence.Models;

namespace PetHome.Persistence;

public class PetHomeDbContext : IdentityDbContext<AppUser>
{
	public DbSet<Owner>? Owners {get;set;}
	public DbSet<Pet>? Pets {get;set;}
	// public DbSet<Transaction>? Transactions {get;set;}
	// public DbSet<Stay>? Stays {get;set;}
	// public DbSet<CareActivity>? Activities {get;set;}
	// public DbSet<Photo>? Photos {get;set;}
	
	public PetHomeDbContext()
	{
	}
	public PetHomeDbContext(DbContextOptions<PetHomeDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) //TODO agregar limite para int, VARCHAR 
	{
		base.OnModelCreating(modelBuilder);
		
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
		
		// modelBuilder.Entity<Pet>()
		// 	.HasMany(m => m.Photos)
		// 	.WithOne(m => m.Pet)
		// 	.HasForeignKey(m => m.PetId)
		// 	.OnDelete(DeleteBehavior.Cascade);
		//
		// modelBuilder.Entity<Stay>()
		// 	.HasMany(s => s.CareActivities)
		// 	.WithOne(a => a.Stay)
		// 	.HasForeignKey(a => a.StayId)
		// 	.OnDelete(DeleteBehavior.Cascade);
		//
		// modelBuilder.Entity<Stay>()
		// 	.HasOne(s => s.Transaction)
		// 	.WithOne(p => p.Stay)
		// 	.HasForeignKey<Transaction>(p => p.StayId)
		// 	.OnDelete(DeleteBehavior.Restrict);
		
		modelBuilder.Entity<Transaction>()
			.Property(t => t.Metadata)
			.HasConversion(
				v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
				v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null)!);

		LoadSecurityData(modelBuilder);
	}
	
	private void LoadSecurityData(ModelBuilder modelBuilder)
    {
        var adminId = Guid.NewGuid().ToString();
        var clientId = Guid.NewGuid().ToString();

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole {
                Id = adminId,
                Name = CustomRoles.ADMIN,
                NormalizedName = CustomRoles.ADMIN
            }
        );

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole {
                Id = clientId,
                Name = CustomRoles.CLIENT,
                NormalizedName = CustomRoles.CLIENT
            }
        );
    
        modelBuilder.Entity<IdentityRoleClaim<string>>()
        .HasData(
            new IdentityRoleClaim<string>{
                Id = 1,
                ClaimType=CustomClaims.POLICIES,
                ClaimValue = PolicyMaster.OWNER_READ,
                RoleId = adminId
            },
             new IdentityRoleClaim<string>{
                Id = 2,
                ClaimType=CustomClaims.POLICIES,
                ClaimValue = PolicyMaster.OWNER_UPDATE,
                RoleId = adminId
            },
             new IdentityRoleClaim<string>{
                Id = 3,
                ClaimType=CustomClaims.POLICIES,
                ClaimValue = PolicyMaster.OWNER_CREATE,
                RoleId = adminId
            },
             new IdentityRoleClaim<string>{
                Id = 4,
                ClaimType=CustomClaims.POLICIES,
                ClaimValue = PolicyMaster.OWNER_DELETE,
                RoleId = adminId
            },
             new IdentityRoleClaim<string>{
                Id = 5,
                ClaimType=CustomClaims.POLICIES,
                ClaimValue = PolicyMaster.PET_CREATE,
                RoleId = adminId
            },
             new IdentityRoleClaim<string>{
                Id = 6,
                ClaimType=CustomClaims.POLICIES,
                ClaimValue = PolicyMaster.PET_READ,
                RoleId = adminId
            },
             new IdentityRoleClaim<string>{
                Id = 7,
                ClaimType=CustomClaims.POLICIES,
                ClaimValue = PolicyMaster.PET_UPDATE,
                RoleId = adminId
            },
            new IdentityRoleClaim<string>{
                Id = 8,
                ClaimType=CustomClaims.POLICIES,
                ClaimValue = PolicyMaster.OWNER_READ,
                RoleId = clientId
            },
            new IdentityRoleClaim<string>{
                Id = 9,
                ClaimType=CustomClaims.POLICIES,
                ClaimValue = PolicyMaster.PET_READ,
                RoleId = clientId
            }
        );
    }
}
