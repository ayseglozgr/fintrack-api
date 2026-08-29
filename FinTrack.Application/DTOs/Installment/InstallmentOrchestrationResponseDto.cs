namespace FinTrack.Application.DTOs.Installment;

public class InstallmentOrchestrationResponseDto
{
    public string InstallmentPlanUid { get; set; } = string.Empty;
    public List<string> InstallmentScheduleUids { get; set; } = new();
}