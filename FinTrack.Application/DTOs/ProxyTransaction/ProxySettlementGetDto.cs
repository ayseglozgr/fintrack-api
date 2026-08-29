namespace FinTrack.Application.DTOs.ProxyTransaction;

public class ProxySettlementGetDto
{
    public string Uid { get; set; } = string.Empty;
    public string ProxyCaseUid { get; set; } = string.Empty;
    public DateTime SettlementDate { get; set; }
    public decimal Amount { get; set; }
    public int PaymentChannel { get; set; }
    public int EntryState { get; set; }
    public string? ReceivedFinancialAccountUid { get; set; }
    public string? SettlementTransactionUid { get; set; }
    public string? Note { get; set; }
}