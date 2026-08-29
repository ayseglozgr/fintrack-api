using FinTrack.Application.Common.Helpers;
using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.Household;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Services
{
    public class HouseholdService : IHouseholdService
    {
        private readonly IHouseholdRepository _householdRepository;

        public HouseholdService(IHouseholdRepository householdRepository)
        {
            _householdRepository = householdRepository;
        }

        public async Task<ServiceResponse<HouseholdDto>> CreateHouseholdAsync(CreateHouseholdDto createHouseholdDto)
        {
            if (createHouseholdDto == null)
            {
                return ServiceResponse<HouseholdDto>.Failure("Household payload is required.");
            }

            if (string.IsNullOrWhiteSpace(createHouseholdDto.Name))
            {
                return ServiceResponse<HouseholdDto>.Failure("Household name is required.");
            }

            var household = new Household
            {
                Name = createHouseholdDto.Name.Trim()
            };

            await _householdRepository.AddAsync(household);

            return ServiceResponse<HouseholdDto>.Success(new HouseholdDto
            {
                Uid = CipherHelper.EncryptId(household.Id),
                Name = household.Name,
                CreateDate = household.CreateDate
            }, "Household created successfully.");
        }

        public async Task<ServiceResponse<HouseholdDto>> UpdateHouseholdAsync(UpdateHouseholdDto updateHouseholdDto)
        {
            if (updateHouseholdDto == null)
            {
                return ServiceResponse<HouseholdDto>.Failure("Household payload is required.");
            }

            if (string.IsNullOrWhiteSpace(updateHouseholdDto.Name))
            {
                return ServiceResponse<HouseholdDto>.Failure("Household name is required.");
            }

            var id = CipherHelper.DecryptId(updateHouseholdDto.Uid);
            if (id <= 0)
            {
                return ServiceResponse<HouseholdDto>.Failure("Invalid encrypted id.");
            }

            var household = await _householdRepository.GetByIdAsync(id);
            if (household == null || household.IsDeleted)
            {
                return ServiceResponse<HouseholdDto>.Failure($"Household not found. Id: {id}");
            }

            household.Name = updateHouseholdDto.Name.Trim();
            household.EditDate = DateTime.UtcNow;
            household.EditUser = "System";

            await _householdRepository.UpdateAsync(household);

            return ServiceResponse<HouseholdDto>.Success(new HouseholdDto
            {
                Uid = CipherHelper.EncryptId(household.Id),
                Name = household.Name,
                CreateDate = household.CreateDate
            }, "Household updated successfully.");
        }
    }
}