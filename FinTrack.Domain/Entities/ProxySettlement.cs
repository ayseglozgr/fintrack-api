using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

/// <summary>
/// Emanet/proxy dosyasına yapılan tahsilat hareketlerini tutar.
/// Tek seferde veya parça parça tahsilatların zaman içinde izlenmesini sağlar.
/// </summary>
public class ProxySettlement : BaseEntity
{
    public int ProxyCaseId { get; set; }
    public ProxyCase ProxyCase { get; set; } = null!;

    /// <summary>
    /// Üçüncü kişiden ödemenin alındığı tarihtir.
    /// </summary>
    public DateTime SettlementDate { get; set; }

    /// <summary>
    /// İlgili tahsilat satırındaki tutardır.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Tahsilatın hangi kanal ile yapıldığını belirtir (nakit/havale vb.).
    /// </summary>
    public PaymentChannel PaymentChannel { get; set; }

    /// <summary>
    /// Tahsilat kaydının gerçekleşmiş mi taslak mı olduğunu belirtir.
    /// </summary>
    public EntryState EntryState { get; set; }

    /// <summary>
    /// Tahsil edilen tutarın aktarıldığı finansal hesap kimliğidir.
    /// </summary>
    public int? ReceivedFinancialAccountId { get; set; }
    public FinancialAccount? ReceivedFinancialAccount { get; set; }

    /// <summary>
    /// Tahsilat için oluşturulan LedgerTransaction kaydı ile ilişki kurar.
    /// Null olması, henüz muhasebe hareketine bağlanmadığı anlamına gelir.
    /// </summary>
    public int? SettlementTransactionId { get; set; }
    public LedgerTransaction? SettlementTransaction { get; set; }

    /// <summary>
    /// Tahsilat satırına ait kullanıcı notudur.
    /// </summary>
    public string? Note { get; set; }
}