using System.ComponentModel.DataAnnotations;
using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Application.DTOs.FinancialAccount;

public class UpdateFinancialAccountRequestDto
{
    [Required]
    public string Uid { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
    public int UserId { get; set; }

    [Range(1, 3, ErrorMessage = "Type must be a valid FinancialAccountType value.")]
    public FinancialAccountType Type { get; set; }

    [Required]
    public string ProviderName { get; set; } = string.Empty;

    [Required]
    public string Alias { get; set; } = string.Empty;

    public string? Last4 { get; set; }

    public bool IsActive { get; set; } = true;
}
