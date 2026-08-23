using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.Household;

namespace FinTrack.Application.Interfaces.Factories;

public interface IUserHouseHoldFactory
{
    Task<ServiceResponse<HouseholdDto>> CreateHouseholdForUserAsync(CreateHouseholdDto createHouseholdDto);
}
