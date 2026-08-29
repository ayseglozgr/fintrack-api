using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

public class FinancialAccount : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public FinancialAccountType Type { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string? Last4 { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<LedgerTransaction> LedgerTransactions { get; set; } = new List<LedgerTransaction>();
    public ICollection<InstallmentPlan> InstallmentPlans { get; set; } = new List<InstallmentPlan>();
    public ICollection<ProxyCase> ProxyCases { get; set; } = new List<ProxyCase>();
    public ICollection<ProxySettlement> ReceivedProxySettlements { get; set; } = new List<ProxySettlement>();
}
