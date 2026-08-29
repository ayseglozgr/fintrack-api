using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.Household;

namespace FinTrack.Application.Interfaces.Services;

public interface IUserService
{
    Task<bool> UserExistsAsync(int userId);
    Task<bool> AddMembershipAsync(int userId, int householdId, bool setAsActive = false);
    Task<bool> IsUserMemberOfHouseholdAsync(int userId, int householdId);
    Task<int?> GetActiveHouseholdIdAsync(int userId);
    Task<ServiceResponse> SwitchActiveHouseholdAsync(int userId, int householdId);
    Task<ServiceResponse<IEnumerable<UserHouseholdMembershipDto>>> GetUserHouseholdsAsync(int userId);
}
