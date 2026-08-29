using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

public class ProxySettlement : BaseEntity
{
    public int ProxyCaseId { get; set; }
    public ProxyCase ProxyCase { get; set; } = null!;

    public DateTime SettlementDate { get; set; }
    public decimal Amount { get; set; }

    public PaymentChannel PaymentChannel { get; set; }
    public EntryState EntryState { get; set; }

    public int? ReceivedFinancialAccountId { get; set; }
    public FinancialAccount? ReceivedFinancialAccount { get; set; }

    public int? SettlementTransactionId { get; set; }
    public LedgerTransaction? SettlementTransaction { get; set; }

    public string? Note { get; set; }
}