namespace FinTrack.Domain.Entities;

/// <summary>
/// Taksitli bir harcamanın ana plan kaydıdır.
/// Toplam tutar, taksit sayısı ve ilk vade bilgisi üzerinden aylık taksit satırları üretilir.
/// </summary>
public class InstallmentPlan : BaseEntity
{
    public int UserHouseholdId { get; set; }
    public UserHousehold UserHousehold { get; set; } = null!;

    public int FinancialAccountId { get; set; }
    public FinancialAccount FinancialAccount { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Taksitli alışverişin yapıldığı tarihtir.
    /// </summary>
    public DateTime PurchaseDate { get; set; }

    /// <summary>
    /// Taksit planının toplam harcama tutarıdır.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Planlanan toplam taksit adedidir (ör. 3, 6, 12).
    /// </summary>
    public short InstallmentCount { get; set; }

    /// <summary>
    /// İlk taksidin vade tarihidir. Sonraki taksitler bu tarihten aylık türetilir.
    /// </summary>
    public DateTime FirstDueDate { get; set; }

    /// <summary>
    /// Harcamanın yapıldığı iş yeri/marka bilgisidir.
    /// </summary>
    public string? MerchantName { get; set; }

    /// <summary>
    /// Plan için kullanıcı açıklaması veya not bilgisidir.
    /// </summary>
    public string? Description { get; set; }

    public ICollection<InstallmentSchedule> Schedules { get; set; } = new List<InstallmentSchedule>();
}