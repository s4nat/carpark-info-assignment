using Microsoft.EntityFrameworkCore;
using CarParkInfo.Core.Models;

namespace CarParkInfo.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<CarPark> CarParks { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserFavorite> UserFavorites { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure CarPark entity
        modelBuilder.Entity<CarPark>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CarParkNo).IsUnique();
            
            // Add performance indexes
            entity.HasIndex(e => e.FreeParking);
            entity.HasIndex(e => e.NightParking);
            entity.HasIndex(e => e.GantryHeight);
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure UserFavorite entity
        modelBuilder.Entity<UserFavorite>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.CarParkNo }).IsUnique();

            entity.HasOne(d => d.User)
                .WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId);

            entity.HasOne(d => d.CarPark)
                .WithMany()
                .HasForeignKey(d => d.CarParkNo)
                .HasPrincipalKey(e => e.CarParkNo);
        });
    }
}