using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OFinance.Domain.Entities;

namespace OFinance.Inrastructure.Data
{

    public class ItemDbContext : IdentityDbContext<IdentityUser>
    {

        public ItemDbContext(DbContextOptions<ItemDbContext> options)
     : base(options)
        {
        }

        public DbSet<Item> Items => Set<Item>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Item>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            //    entity.Property(e => e.Description).HasMaxLength(1000);
            //    entity.Property(e => e.CreatedAt).IsRequired();
            //});

           

        }
    }
}
