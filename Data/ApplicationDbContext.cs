using Microsoft.EntityFrameworkCore;
using BlogPortal.Models;

namespace BlogPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Blog> Blogs => Set<Blog>();

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(entry => entry.Entity is BaseEntity &&
                    (entry.State == EntityState.Added || entry.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.UpdatedAt = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }
    }
}
