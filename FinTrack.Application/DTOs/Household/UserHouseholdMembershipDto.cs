namespace FinTrack.Application.DTOs.Household;

public class UserHouseholdMembershipDto
{
    public string UserUid { get; set; } = string.Empty;
    public string HouseholdUid { get; set; } = string.Empty;
    public string HouseholdName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime JoinedAt { get; set; }
    public string MembershipStatus { get; set; } = string.Empty;
    public string MemberRole { get; set; } = string.Empty;
}