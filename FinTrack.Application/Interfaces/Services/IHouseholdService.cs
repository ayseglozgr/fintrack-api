using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.Household;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrack.Application.Interfaces.Services
{
    public interface IHouseholdService
    {
        Task<ServiceResponse<HouseholdDto>> CreateHouseholdAsync(CreateHouseholdDto createHouseholdDto);
        Task<ServiceResponse<HouseholdDto>> UpdateHouseholdAsync(UpdateHouseholdDto updateHouseholdDto);
    }
}
