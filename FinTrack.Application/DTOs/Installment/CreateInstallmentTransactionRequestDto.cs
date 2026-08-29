using System.ComponentModel.DataAnnotations;
using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Application.DTOs.Installment;

public class CreateInstallmentTransactionRequestDto
{
    [Required]
    public string UserHouseholdUid { get; set; } = string.Empty;

    [Required]
    public string FinancialAccountUid { get; set; } = string.Empty;

    [Required]
    public string CategoryUid { get; set; } = string.Empty;

    public DateTime PurchaseDate { get; set; }

    [Range(typeof(decimal), "0.01", "999999999999")]
    public decimal TotalAmount { get; set; }

    [Range(1, short.MaxValue)]
    public short InstallmentCount { get; set; }

    public DateTime FirstDueDate { get; set; }

    public string? MerchantName { get; set; }
    public string? Description { get; set; }

    [Range(1, 4)]
    public PaymentChannel PaymentChannel { get; set; } = PaymentChannel.CreditCard;

    [Range(1, 2)]
    public EntryState EntryState { get; set; } = EntryState.DraftProjected;
}