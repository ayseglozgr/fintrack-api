using FinTrack.Application.DTOs.Household;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrack.Application.Interfaces.Services
{
    public interface IHouseholdService
    {
        Task<HouseholdDto> CreateHouseholdAsync(CreateHouseholdDto createHouseholdDto);
    }
}
