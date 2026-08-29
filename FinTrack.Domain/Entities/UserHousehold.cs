namespace FinTrack.Domain.Entities;

public class UserHousehold : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int HouseholdId { get; set; }
    public Household Household { get; set; } = null!;

    public bool IsActive { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public string MembershipStatus { get; set; } = "Active";
    public string MemberRole { get; set; } = "Member";

    public ICollection<LedgerTransaction> LedgerTransactions { get; set; } = new List<LedgerTransaction>();
    public ICollection<InstallmentPlan> InstallmentPlans { get; set; } = new List<InstallmentPlan>();
    public ICollection<ProxyCase> ProxyCases { get; set; } = new List<ProxyCase>();
}