using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

public class LedgerTransaction : BaseEntity
{
    public int UserHouseholdId { get; set; }
    public UserHousehold UserHousehold { get; set; } = null!;

    public int? FinancialAccountId { get; set; }
    public FinancialAccount? FinancialAccount { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }
    public PaymentChannel PaymentChannel { get; set; }
    public EntryState EntryState { get; set; }
    public ExpenseKind ExpenseKind { get; set; } = ExpenseKind.Normal;

    public bool IsBudgetNeutral { get; set; } = false;
    public string? Description { get; set; }
}