using System.ComponentModel.DataAnnotations;
using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Application.DTOs.LedgerTransaction;

public class CreateLedgerTransactionRequestDto
{
    [Required]
    public string UserHouseholdUid { get; set; } = string.Empty;

    public string? FinancialAccountUid { get; set; }

    [Required]
    public string CategoryUid { get; set; } = string.Empty;

    public DateTime TransactionDate { get; set; }

    [Range(typeof(decimal), "0.01", "999999999999")]
    public decimal Amount { get; set; }

    [Range(1, 3)]
    public TransactionType TransactionType { get; set; }

    [Range(1, 4)]
    public PaymentChannel PaymentChannel { get; set; }

    [Range(1, 2)]
    public EntryState EntryState { get; set; }

    [Range(1, 2)]
    public ExpenseKind ExpenseKind { get; set; } = ExpenseKind.Normal;

    public bool IsBudgetNeutral { get; set; } = false;
    public string? Description { get; set; }
}