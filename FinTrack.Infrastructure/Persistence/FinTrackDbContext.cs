using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public class FinTrackDbContext : DbContext
{
    public FinTrackDbContext(DbContextOptions<FinTrackDbContext> options) : base(options)
    {
    }

    public DbSet<User> User => Set<User>();
    public DbSet<Household> Household => Set<Household>();
    public DbSet<UserHousehold> UserHousehold => Set<UserHousehold>();
    public DbSet<FinancialAccount> FinancialAccount => Set<FinancialAccount>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<LedgerTransaction> LedgerTransaction => Set<LedgerTransaction>();
    public DbSet<InstallmentPlan> InstallmentPlan => Set<InstallmentPlan>();
    public DbSet<InstallmentSchedule> InstallmentSchedule => Set<InstallmentSchedule>();
    public DbSet<ProxyCase> ProxyCase => Set<ProxyCase>();
    public DbSet<ProxySettlement> ProxySettlement => Set<ProxySettlement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tablo isimlerini açıkça veritabanındaki tekil isimlerle eşliyoruz
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Household>().ToTable("Household");
        modelBuilder.Entity<UserHousehold>().ToTable("UserHousehold");
        modelBuilder.Entity<FinancialAccount>().ToTable("FinancialAccount");
        modelBuilder.Entity<Category>().ToTable("Category");
        modelBuilder.Entity<LedgerTransaction>().ToTable("LedgerTransaction");
        modelBuilder.Entity<InstallmentPlan>().ToTable("InstallmentPlan");
        modelBuilder.Entity<InstallmentSchedule>().ToTable("InstallmentSchedule");
        modelBuilder.Entity<ProxyCase>().ToTable("ProxyCase");
        modelBuilder.Entity<ProxySettlement>().ToTable("ProxySettlement");

        modelBuilder.Entity<User>()
            .Property(u => u.AccountName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.RefreshTokenHash)
            .HasMaxLength(128);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.AccountName)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UserHousehold>()
            .Property(uh => uh.MembershipStatus)
            .IsRequired()
            .HasMaxLength(30);

        modelBuilder.Entity<UserHousehold>()
            .Property(uh => uh.MemberRole)
            .IsRequired()
            .HasMaxLength(30);

        modelBuilder.Entity<UserHousehold>()
            .HasIndex(uh => new { uh.UserId, uh.HouseholdId })
            .IsUnique();

        modelBuilder.Entity<UserHousehold>()
            .HasIndex(uh => uh.UserId)
            .HasFilter("\"IsActive\" = TRUE AND \"IsDeleted\" = FALSE")
            .IsUnique();

        modelBuilder.Entity<FinancialAccount>()
            .Property(f => f.ProviderName)
            .IsRequired()
            .HasMaxLength(120);

        modelBuilder.Entity<FinancialAccount>()
            .Property(f => f.Alias)
            .IsRequired()
            .HasMaxLength(80);

        modelBuilder.Entity<FinancialAccount>()
            .Property(f => f.Last4)
            .HasMaxLength(4);

        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Category>()
            .HasIndex(c => new { c.HouseholdId, c.ParentCategoryId, c.Name, c.Direction })
            .IsUnique();

        modelBuilder.Entity<LedgerTransaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<LedgerTransaction>()
            .Property(t => t.Description)
            .HasMaxLength(300);

        modelBuilder.Entity<LedgerTransaction>()
            .HasIndex(t => new { t.UserHouseholdId, t.TransactionDate });

        modelBuilder.Entity<LedgerTransaction>()
            .HasIndex(t => new { t.EntryState, t.TransactionDate });

        modelBuilder.Entity<InstallmentPlan>()
            .Property(p => p.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InstallmentPlan>()
            .Property(p => p.MerchantName)
            .HasMaxLength(120);

        modelBuilder.Entity<InstallmentPlan>()
            .Property(p => p.Description)
            .HasMaxLength(300);

        modelBuilder.Entity<InstallmentSchedule>()
            .Property(i => i.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InstallmentSchedule>()
            .HasIndex(i => new { i.InstallmentPlanId, i.InstallmentNo })
            .IsUnique();

        modelBuilder.Entity<ProxyCase>()
            .Property(p => p.ExternalPartyName)
            .IsRequired()
            .HasMaxLength(120);

        modelBuilder.Entity<ProxyCase>()
            .Property(p => p.TotalAdvancedAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProxyCase>()
            .Property(p => p.Description)
            .HasMaxLength(300);

        modelBuilder.Entity<ProxySettlement>()
            .Property(s => s.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProxySettlement>()
            .Property(s => s.Note)
            .HasMaxLength(250);

        modelBuilder.Entity<ProxySettlement>()
            .HasIndex(s => new { s.ProxyCaseId, s.SettlementDate });

        // Global Query Filter'larımız
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Household>().HasQueryFilter(h => !h.IsDeleted);
        modelBuilder.Entity<UserHousehold>().HasQueryFilter(uh => !uh.IsDeleted);
        modelBuilder.Entity<FinancialAccount>().HasQueryFilter(f => !f.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<LedgerTransaction>().HasQueryFilter(t => !t.IsDeleted);
        modelBuilder.Entity<InstallmentPlan>().HasQueryFilter(i => !i.IsDeleted);
        modelBuilder.Entity<InstallmentSchedule>().HasQueryFilter(i => !i.IsDeleted);
        modelBuilder.Entity<ProxyCase>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<ProxySettlement>().HasQueryFilter(s => !s.IsDeleted);

        modelBuilder.Entity<FinancialAccount>()
            .HasOne(f => f.User)
            .WithMany(u => u.FinancialAccounts)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserHousehold>()
            .HasOne(uh => uh.User)
            .WithMany(u => u.UserHouseholds)
            .HasForeignKey(uh => uh.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserHousehold>()
            .HasOne(uh => uh.Household)
            .WithMany(h => h.UserHouseholds)
            .HasForeignKey(uh => uh.HouseholdId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.Household)
            .WithMany(h => h.Categories)
            .HasForeignKey(c => c.HouseholdId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LedgerTransaction>()
            .HasOne(t => t.UserHousehold)
            .WithMany(uh => uh.LedgerTransactions)
            .HasForeignKey(t => t.UserHouseholdId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LedgerTransaction>()
            .HasOne(t => t.FinancialAccount)
            .WithMany(f => f.LedgerTransactions)
            .HasForeignKey(t => t.FinancialAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<LedgerTransaction>()
            .HasOne(t => t.Category)
            .WithMany(c => c.LedgerTransactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InstallmentPlan>()
            .HasOne(p => p.UserHousehold)
            .WithMany(uh => uh.InstallmentPlans)
            .HasForeignKey(p => p.UserHouseholdId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InstallmentPlan>()
            .HasOne(p => p.FinancialAccount)
            .WithMany(f => f.InstallmentPlans)
            .HasForeignKey(p => p.FinancialAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InstallmentPlan>()
            .HasOne(p => p.Category)
            .WithMany(c => c.InstallmentPlans)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InstallmentSchedule>()
            .HasOne(i => i.InstallmentPlan)
            .WithMany(p => p.Schedules)
            .HasForeignKey(i => i.InstallmentPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InstallmentSchedule>()
            .HasOne(i => i.ActualTransaction)
            .WithMany()
            .HasForeignKey(i => i.ActualTransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ProxyCase>()
            .HasOne(p => p.UserHousehold)
            .WithMany(uh => uh.ProxyCases)
            .HasForeignKey(p => p.UserHouseholdId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProxyCase>()
            .HasOne(p => p.FinancialAccount)
            .WithMany(f => f.ProxyCases)
            .HasForeignKey(p => p.FinancialAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProxySettlement>()
            .HasOne(s => s.ProxyCase)
            .WithMany(p => p.Settlements)
            .HasForeignKey(s => s.ProxyCaseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProxySettlement>()
            .HasOne(s => s.ReceivedFinancialAccount)
            .WithMany(f => f.ReceivedProxySettlements)
            .HasForeignKey(s => s.ReceivedFinancialAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ProxySettlement>()
            .HasOne(s => s.SettlementTransaction)
            .WithMany()
            .HasForeignKey(s => s.SettlementTransactionId)
            .OnDelete(DeleteBehavior.SetNull);

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