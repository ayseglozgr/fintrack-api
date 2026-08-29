using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

public class Category : BaseEntity
{
    public int? HouseholdId { get; set; }
    public Household? Household { get; set; }

    public string Name { get; set; } = string.Empty;

    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();

    public LedgerDirection Direction { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<LedgerTransaction> LedgerTransactions { get; set; } = new List<LedgerTransaction>();
    public ICollection<InstallmentPlan> InstallmentPlans { get; set; } = new List<InstallmentPlan>();
}