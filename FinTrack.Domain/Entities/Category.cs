using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

/// <summary>
/// Gelir-gider hareketlerinin sınıflandırılmasını sağlayan kategori tanımıdır.
/// Hane bazlı (HouseholdId dolu) veya sistem genel (HouseholdId null) kullanılabilir.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Kategori haneye özel ise ilgili hane kimliğini tutar.
    /// Null olduğunda kategori sistem genel kabul edilir.
    /// </summary>
    public int? HouseholdId { get; set; }
    public Household? Household { get; set; }

    /// <summary>
    /// Kategori görünen adıdır (ör. Kira, Market, Maaş).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Alt kategori yapısı için üst kategorinin kimliğidir.
    /// Null ise kök kategori anlamına gelir.
    /// </summary>
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();

    /// <summary>
    /// Kategorinin gelir (Inflow) mi gider (Outflow) mi olduğunu belirtir.
    /// </summary>
    public LedgerDirection Direction { get; set; }

    /// <summary>
    /// Kategorinin aktif kullanımda olup olmadığını belirtir.
    /// </summary>
    public bool IsActive { get; set; } = true;

    public ICollection<LedgerTransaction> LedgerTransactions { get; set; } = new List<LedgerTransaction>();
    public ICollection<InstallmentPlan> InstallmentPlans { get; set; } = new List<InstallmentPlan>();
}