using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

/// <summary>
/// Emanet/proxy harcama dosyasını temsil eder.
/// Hane üyesinin kendi hesabından üçüncü kişi adına yaptığı harcamanın alacak takibini tutar.
/// </summary>
public class ProxyCase : BaseEntity
{
    public int UserHouseholdId { get; set; }
    public UserHousehold UserHousehold { get; set; } = null!;

    public int FinancialAccountId { get; set; }
    public FinancialAccount FinancialAccount { get; set; } = null!;

    /// <summary>
    /// Harcamanın yapıldığı üçüncü kişi adıdır (ör. arkadaş, akraba).
    /// </summary>
    public string ExternalPartyName { get; set; } = string.Empty;

    /// <summary>
    /// Karttan/hesaptan emanet harcamanın yapıldığı tarihtir.
    /// </summary>
    public DateTime SpentDate { get; set; }

    /// <summary>
    /// Üçüncü kişi adına yapılan toplam avans harcama tutarıdır.
    /// </summary>
    public decimal TotalAdvancedAmount { get; set; }

    /// <summary>
    /// Alacak dosyasının tahsilat durumunu belirtir (açık, kısmi, kapalı, terkin).
    /// </summary>
    public SettlementStatus Status { get; set; } = SettlementStatus.Open;

    /// <summary>
    /// Dosya tamamen kapandığında tahsilat kapanış tarihini tutar.
    /// </summary>
    public DateTime? ClosedDate { get; set; }

    /// <summary>
    /// Emanet işlemi için ek açıklama/not alanıdır.
    /// </summary>
    public string? Description { get; set; }

    public ICollection<ProxySettlement> Settlements { get; set; } = new List<ProxySettlement>();
}