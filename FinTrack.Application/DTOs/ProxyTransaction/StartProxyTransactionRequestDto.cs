using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.DTOs.ProxyTransaction;

public class StartProxyTransactionRequestDto
{
    [Required]
    public string UserHouseholdUid { get; set; } = string.Empty;

    [Required]
    public string FinancialAccountUid { get; set; } = string.Empty;

    [Required]
    public string ExternalPartyName { get; set; } = string.Empty;

    public DateTime SpentDate { get; set; }

    [Range(typeof(decimal), "0.01", "999999999999")]
    public decimal TotalAdvancedAmount { get; set; }

    public string? Description { get; set; }
}