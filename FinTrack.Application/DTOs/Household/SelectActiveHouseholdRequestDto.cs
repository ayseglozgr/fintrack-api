using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.DTOs.Household;

public class SelectActiveHouseholdRequestDto
{
    [Required]
    public string UserUid { get; set; } = string.Empty;

    [Required]
    public string HouseholdUid { get; set; } = string.Empty;
}