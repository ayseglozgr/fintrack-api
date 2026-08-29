using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

/// <summary>
/// Bir taksit planının aylara dağıtılmış satırlarını tutar.
/// Her satır belirli bir vade, sıra numarası ve tutar için tek taksidi temsil eder.
/// </summary>
public class InstallmentSchedule : BaseEntity
{
    public int InstallmentPlanId { get; set; }
    public InstallmentPlan InstallmentPlan { get; set; } = null!;

    /// <summary>
    /// Taksidin plan içindeki sıra numarasıdır (1..N).
    /// </summary>
    public short InstallmentNo { get; set; }

    /// <summary>
    /// Taksidin ilgili ay için beklenen/gerçekleşen vade tarihidir.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// İlgili taksit satırının tutarıdır.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Satırın gerçekleşmiş (Actual) mi, ileriye dönük taslak (DraftProjected) mı olduğunu belirtir.
    /// </summary>
    public EntryState EntryState { get; set; }

    /// <summary>
    /// Satırın operasyonel durumudur (taslak, gerçekleşmiş, iptal vb.).
    /// </summary>
    public StatusType StatusType { get; set; } = StatusType.DraftProjected;

    /// <summary>
    /// Taksit gerçekten işlendiğinde oluşan muhasebe kaydının kimliğidir.
    /// Null ise satır henüz fiili işleme bağlanmamıştır.
    /// </summary>
    public int? ActualTransactionId { get; set; }
    public LedgerTransaction? ActualTransaction { get; set; }
}