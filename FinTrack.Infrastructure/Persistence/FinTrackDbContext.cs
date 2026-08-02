using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public class FinTrackDbContext : DbContext
{
    public FinTrackDbContext(DbContextOptions<FinTrackDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Household> Households => Set<Household>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tablo isimlerini açıkça veritabanındaki tekil isimlerle eşliyoruz
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Household>().ToTable("Household");

        // Global Query Filter'larımız
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Household>().HasQueryFilter(h => !h.IsDeleted);

        // Kullanıcının bir hanesi olmak zorunda değil (IsRequired(false))
        modelBuilder.Entity<Household>()
            .HasMany(h => h.User)
            .WithOne(u => u.Household)
            .HasForeignKey(u => u.HouseholdId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreateDate = DateTime.UtcNow;
                    entry.Entity.CreateUser = "System";
                    entry.Entity.IsDeleted = false;
                    break;

                case EntityState.Modified:
                    entry.Entity.EditDate = DateTime.UtcNow;
                    entry.Entity.EditUser = "System";
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}