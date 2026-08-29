namespace FinTrack.Application.DTOs.Household;

public class UserHouseholdMembershipDto
{
    public int UserId { get; set; }
    public int HouseholdId { get; set; }
    public string HouseholdName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime JoinedAt { get; set; }
    public string MembershipStatus { get; set; } = string.Empty;
    public string MemberRole { get; set; } = string.Empty;
}