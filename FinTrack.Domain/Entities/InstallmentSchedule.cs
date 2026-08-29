using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

public class InstallmentSchedule : BaseEntity
{
    public int InstallmentPlanId { get; set; }
    public InstallmentPlan InstallmentPlan { get; set; } = null!;

    public short InstallmentNo { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }

    public EntryState EntryState { get; set; }
    public StatusType StatusType { get; set; } = StatusType.DraftProjected;

    public int? ActualTransactionId { get; set; }
    public LedgerTransaction? ActualTransaction { get; set; }
}