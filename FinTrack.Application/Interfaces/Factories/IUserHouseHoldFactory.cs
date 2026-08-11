using FinTrack.Application.DTOs.Household;

namespace FinTrack.Application.Interfaces.Factories;

public interface IUserHouseHoldFactory
{
    Task<HouseholdDto> CreateHouseholdForUserAsync(CreateHouseholdDto createHouseholdDto);
}
