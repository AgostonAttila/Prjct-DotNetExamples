using CleanFromScratch.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanFromScratch.Infrastructure.Persistence
{
    internal class RestaurantDbContext(DbContextOptions<RestaurantDbContext> option) : IdentityDbContext<User>(option)
    {
        internal DbSet<Restaurant> Restaurants { get; set; }
        internal DbSet<Dish> Dishes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurant>()
                .OwnsOne(e => e.Address);

            modelBuilder.Entity<Restaurant>()
             .HasMany(e => e.Dishes)
             .WithOne()
             .HasForeignKey(d =>d.RestaurantId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OwnedRestaurants)
                .WithOne(e => e.Owner)
                .HasForeignKey(e => e.OwnerId);

        }

    }
}
