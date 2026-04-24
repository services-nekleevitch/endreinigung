using EndReinigung.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EndReinigung.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings");
                entity.HasIndex(b => b.BookingNumber).IsUnique();
                entity.HasIndex(b => b.CreatedAt);
                entity.HasIndex(b => b.Email);
                entity.Property(b => b.Status).HasConversion<int>();
            });
        }
    }
}
