using DbAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DbAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {                
        }

        public DbSet<PriceConfigurationSet> PriceConfigurationSets { get; set; }
        public DbSet<PriceConfigurationEntry> PriceConfigurationEntries { get; set; }
        public DbSet<LocationCH> LocationsCH { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PriceConfigurationEntry>()
                .HasIndex(p => new { p.PriceConfigurationSetId, p.Category, p.OptionKey })
                .IsUnique();

            modelBuilder.Entity<PriceConfigurationEntry>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<LocationCH>(entity =>
            {
                entity.ToTable("LocationsCH");
                entity.Property(e => e.ZipCode).HasMaxLength(10).IsRequired();
                entity.Property(e => e.PlaceName).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Canton).HasMaxLength(2);
            });
        }
    }
}
