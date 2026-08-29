namespace FinTrack.Domain.Entities;

public class InstallmentPlan : BaseEntity
{
    public int UserHouseholdId { get; set; }
    public UserHousehold UserHousehold { get; set; } = null!;

    public int FinancialAccountId { get; set; }
    public FinancialAccount FinancialAccount { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public short InstallmentCount { get; set; }
    public DateTime FirstDueDate { get; set; }

    public string? MerchantName { get; set; }
    public string? Description { get; set; }

    public ICollection<InstallmentSchedule> Schedules { get; set; } = new List<InstallmentSchedule>();
}