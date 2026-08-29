using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

/// <summary>
/// Hanedeki tüm finansal hareketlerin (gelir, gider, transfer) ana kayıt varlığıdır.
/// Nakit, banka, kredi kartı ve yemek kartı kanallarından gelen işlemleri tek modelde toplar.
/// </summary>
public class LedgerTransaction : BaseEntity
{
    public int UserHouseholdId { get; set; }
    public UserHousehold UserHousehold { get; set; } = null!;

    public int? FinancialAccountId { get; set; }
    public FinancialAccount? FinancialAccount { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    /// <summary>
    /// İşlemin finansal olarak gerçekleştiği tarihtir.
    /// Draft/Actual kuralı bu tarih üzerinden değerlendirilir.
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// İşlem tutarıdır. Pozitif değer tutulur, yön bilgisi TransactionType ile taşınır.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Hareketin gelir, gider veya transfer olduğunu belirtir.
    /// </summary>
    public TransactionType TransactionType { get; set; }

    /// <summary>
    /// Ödeme/tahsilat kanalını belirtir (nakit, havale, kredi kartı, yemek kartı).
    /// </summary>
    public PaymentChannel PaymentChannel { get; set; }

    /// <summary>
    /// Kaydın gerçekleşmiş (Actual) mi, plan/taslak (DraftProjected) mı olduğunu belirtir.
    /// Geçmiş tarihli kayıtlarda Actual olması beklenir.
    /// </summary>
    public EntryState EntryState { get; set; }

    /// <summary>
    /// Giderin normal tüketim mi yoksa emanet/proxy avans harcaması mı olduğunu belirtir.
    /// </summary>
    public ExpenseKind ExpenseKind { get; set; } = ExpenseKind.Normal;

    /// <summary>
    /// İşlem hanenin net bütçe hesabına dahil edilmeyecekse true olur.
    /// Özellikle emanet/proxy harcamalarda bütçe çarpılmasını önlemek için kullanılır.
    /// </summary>
    public bool IsBudgetNeutral { get; set; } = false;

    /// <summary>
    /// Kullanıcı tarafından girilen serbest metin işlem açıklamasıdır.
    /// </summary>
    public string? Description { get; set; }
}