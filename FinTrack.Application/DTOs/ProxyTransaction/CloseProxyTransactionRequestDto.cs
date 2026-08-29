using System.ComponentModel.DataAnnotations;
using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Application.DTOs.ProxyTransaction;

public class CloseProxyTransactionRequestDto
{
    [Required]
    public string ProxyCaseUid { get; set; } = string.Empty;

    public DateTime SettlementDate { get; set; }

    [Range(typeof(decimal), "0.01", "999999999999")]
    public decimal Amount { get; set; }

    [Range(1, 4)]
    public PaymentChannel PaymentChannel { get; set; }

    [Range(1, 2)]
    public EntryState EntryState { get; set; } = EntryState.Actual;

    public string? ReceivedFinancialAccountUid { get; set; }
    public string? Note { get; set; }
}