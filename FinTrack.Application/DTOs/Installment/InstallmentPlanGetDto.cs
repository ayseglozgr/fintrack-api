namespace FinTrack.Application.DTOs.Installment;

public class InstallmentPlanGetDto
{
    public string Uid { get; set; } = string.Empty;
    public string UserHouseholdUid { get; set; } = string.Empty;
    public string FinancialAccountUid { get; set; } = string.Empty;
    public string CategoryUid { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public short InstallmentCount { get; set; }
    public DateTime FirstDueDate { get; set; }
    public string? MerchantName { get; set; }
    public string? Description { get; set; }
    public List<InstallmentScheduleGetDto> Schedules { get; set; } = new();
}