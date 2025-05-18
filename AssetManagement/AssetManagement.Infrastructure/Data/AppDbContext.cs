using AssetManagement.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<Account, IdentityRole<Guid>, Guid>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<ReturningRequest> ReturnRequests { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Renaming Identity Tables
        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");

        // Mapping Entities Configuration
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}