using FinTrack.Application.Interfaces.Factories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Application.DTOs.Household;

namespace FinTrack.Infrastructure.Factories;

public class UserHouseHoldFactory : IUserHouseHoldFactory
{
    private readonly IHouseholdService _householdService;
    private readonly IUserService _userService;

    public UserHouseHoldFactory(IHouseholdService householdService, IUserService userService)
    {
        _householdService = householdService;
        _userService = userService;
    }

    public async Task<HouseholdDto> CreateHouseholdForUserAsync(CreateHouseholdDto createHouseholdDto)
    {
        var userExists = await _userService.UserExistsAsync(createHouseholdDto.UserId);
        if (!userExists)
        {
            throw new InvalidOperationException($"User not found. UserId: {createHouseholdDto.UserId}");
        }

        var household = await _householdService.CreateHouseholdAsync(createHouseholdDto);

        var isAssigned = await _userService.AssignHouseholdAsync(createHouseholdDto.UserId, household.Id);
        if (!isAssigned)
        {
            throw new InvalidOperationException("User-household assignment failed.");
        }

        return household;
    }
}
