namespace FinTrack.Application.DTOs.FinancialAccount;

public class FinancialAccountGetDto
{
    public string Uid { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int Type { get; set; }
    public string TypeDescriptionTr { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string? Last4 { get; set; }
    public bool IsActive { get; set; }
}
