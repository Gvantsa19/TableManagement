using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.Entities;

namespace TMS.Infrastructure.Persistence
{
    public class TMSDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public TMSDbContext(DbContextOptions<TMSDbContext> options) : base(options)
        {
            
        }
        public DbSet<DynamicTable> Tables { get; set; }
        public DbSet<ColumnInfo> Columns { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DynamicTable>()
                .HasMany(dt => dt.Columns)
                .WithOne(c => c.Table)
                .HasForeignKey(c => c.TableId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public override int SaveChanges() => SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
