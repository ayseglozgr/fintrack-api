namespace FinTrack.Application.DTOs.ProxyTransaction;

public class ProxyCaseGetDto
{
    public string Uid { get; set; } = string.Empty;
    public string UserHouseholdUid { get; set; } = string.Empty;
    public string FinancialAccountUid { get; set; } = string.Empty;
    public string ExternalPartyName { get; set; } = string.Empty;
    public DateTime SpentDate { get; set; }
    public decimal TotalAdvancedAmount { get; set; }
    public int Status { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? Description { get; set; }
}