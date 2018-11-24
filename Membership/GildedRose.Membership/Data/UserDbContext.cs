using GildedRose.Membership.Entities;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.Membership.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users", "membership");

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    property.Relational().ColumnName = property.Relational().ColumnName;
                }
            }

            modelBuilder.Entity<User>().HasKey(x => new { x.Id });
        }
    }
}
