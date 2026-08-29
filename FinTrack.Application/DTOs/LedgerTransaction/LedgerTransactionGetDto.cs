namespace FinTrack.Application.DTOs.LedgerTransaction;

public class LedgerTransactionGetDto
{
    public string Uid { get; set; } = string.Empty;
    public string UserHouseholdUid { get; set; } = string.Empty;
    public string? FinancialAccountUid { get; set; }
    public string CategoryUid { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public int TransactionType { get; set; }
    public int PaymentChannel { get; set; }
    public int EntryState { get; set; }
    public int ExpenseKind { get; set; }
    public bool IsBudgetNeutral { get; set; }
    public string? Description { get; set; }
}