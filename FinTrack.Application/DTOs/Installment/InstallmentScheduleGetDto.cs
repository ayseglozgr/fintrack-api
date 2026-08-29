namespace FinTrack.Application.DTOs.Installment;

public class InstallmentScheduleGetDto
{
    public string Uid { get; set; } = string.Empty;
    public string InstallmentPlanUid { get; set; } = string.Empty;
    public short InstallmentNo { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public int EntryState { get; set; }
    public int StatusType { get; set; }
    public string? ActualTransactionUid { get; set; }
}