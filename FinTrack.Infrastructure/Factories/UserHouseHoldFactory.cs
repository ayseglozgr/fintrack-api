using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.Household;
using FinTrack.Application.Interfaces.Factories;
using FinTrack.Application.Interfaces.Services;

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

    public async Task<ServiceResponse<HouseholdDto>> CreateHouseholdForUserAsync(CreateHouseholdDto createHouseholdDto)
    {
        if (createHouseholdDto == null)
        {
            return ServiceResponse<HouseholdDto>.Failure("Household payload is required.");
        }

        var userExists = await _userService.UserExistsAsync(createHouseholdDto.UserId);
        if (!userExists)
        {
            return ServiceResponse<HouseholdDto>.Failure($"User not found. UserId: {createHouseholdDto.UserId}");
        }

        var householdResponse = await _householdService.CreateHouseholdAsync(createHouseholdDto);
        if (!householdResponse.IsSuccess || householdResponse.Data == null)
        {
            return ServiceResponse<HouseholdDto>.Failure(
                householdResponse.Message,
                householdResponse.Errors);
        }

        var isAssigned = await _userService.AddMembershipAsync(
            createHouseholdDto.UserId,
            householdResponse.Data.Id,
            setAsActive: true);
        if (!isAssigned)
        {
            return ServiceResponse<HouseholdDto>.Failure("User-household assignment failed.");
        }

        return householdResponse;
    }
}
