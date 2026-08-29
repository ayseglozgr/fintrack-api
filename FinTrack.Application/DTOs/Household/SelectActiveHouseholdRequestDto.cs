using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.DTOs.Household;

public class SelectActiveHouseholdRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
    public int UserId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "HouseholdId must be greater than 0.")]
    public int HouseholdId { get; set; }
}