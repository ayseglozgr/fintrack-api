using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Domain.Entities;

public class FinancialAccount : BaseEntity
{
    public int UserId { get; set; }
    public FinancialAccountType Type { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string? Last4 { get; set; }
    public bool IsActive { get; set; } = true;
}
