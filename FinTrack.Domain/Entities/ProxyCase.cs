using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

public class ProxyCase : BaseEntity
{
    public int UserHouseholdId { get; set; }
    public UserHousehold UserHousehold { get; set; } = null!;

    public int FinancialAccountId { get; set; }
    public FinancialAccount FinancialAccount { get; set; } = null!;

    public string ExternalPartyName { get; set; } = string.Empty;
    public DateTime SpentDate { get; set; }
    public decimal TotalAdvancedAmount { get; set; }

    public SettlementStatus Status { get; set; } = SettlementStatus.Open;
    public DateTime? ClosedDate { get; set; }
    public string? Description { get; set; }

    public ICollection<ProxySettlement> Settlements { get; set; } = new List<ProxySettlement>();
}