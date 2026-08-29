using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.Household;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user != null;
    }

    public async Task<bool> AddMembershipAsync(int userId, int householdId, bool setAsActive = false)
    {
        var membership = await _userRepository.GetUserHouseholdAsync(userId, householdId);
        if (membership != null)
        {
            if (setAsActive)
            {
                var switchResponse = await SwitchActiveHouseholdAsync(userId, householdId);
                return switchResponse.IsSuccess;
            }

            return true;
        }

        var userHousehold = new UserHousehold
        {
            UserId = userId,
            HouseholdId = householdId,
            IsActive = false,
            JoinedAt = DateTime.UtcNow,
            MembershipStatus = "Active",
            MemberRole = "Member"
        };

        await _userRepository.AddUserHouseholdAsync(userHousehold);

        var hasActiveMembership = await _userRepository.GetActiveUserHouseholdAsync(userId) != null;
        if (setAsActive || !hasActiveMembership)
        {
            var switchResponse = await SwitchActiveHouseholdAsync(userId, householdId);
            return switchResponse.IsSuccess;
        }

        return true;
    }

    public async Task<bool> IsUserMemberOfHouseholdAsync(int userId, int householdId)
    {
        return await _userRepository.GetUserHouseholdAsync(userId, householdId) != null;
    }

    public async Task<int?> GetActiveHouseholdIdAsync(int userId)
    {
        var activeMembership = await _userRepository.GetActiveUserHouseholdAsync(userId);
        return activeMembership?.HouseholdId;
    }

    public async Task<ServiceResponse> SwitchActiveHouseholdAsync(int userId, int householdId)
    {
        var memberships = await _userRepository.GetUserHouseholdsAsync(userId);
        if (memberships.Count == 0)
        {
            return ServiceResponse.Failure($"User has no household memberships. UserId: {userId}");
        }

        var targetMembership = memberships.FirstOrDefault(uh => uh.HouseholdId == householdId);
        if (targetMembership == null)
        {
            return ServiceResponse.Failure($"User is not a member of household. UserId: {userId}, HouseholdId: {householdId}");
        }

        await _userRepository.SetActiveHouseholdAsync(userId, householdId);

        return ServiceResponse.Success("Active household updated successfully.");
    }

    public async Task<ServiceResponse<IEnumerable<UserHouseholdMembershipDto>>> GetUserHouseholdsAsync(int userId)
    {
        var memberships = await _userRepository.GetUserHouseholdsAsync(userId);

        var mapped = memberships.Select(uh => new UserHouseholdMembershipDto
        {
            UserId = uh.UserId,
            HouseholdId = uh.HouseholdId,
            HouseholdName = uh.Household.Name,
            IsActive = uh.IsActive,
            JoinedAt = uh.JoinedAt,
            MembershipStatus = uh.MembershipStatus,
            MemberRole = uh.MemberRole
        });

        return ServiceResponse<IEnumerable<UserHouseholdMembershipDto>>.Success(mapped);
    }
}
