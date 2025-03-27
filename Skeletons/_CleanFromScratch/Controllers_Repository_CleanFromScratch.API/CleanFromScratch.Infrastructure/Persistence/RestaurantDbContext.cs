using CleanFromScratch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanFromScratch.Infrastructure.Persistence
{
    internal class RestaurantDbContext(DbContextOptions<RestaurantDbContext> option) : DbContext(option)
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

        }

    }
}
