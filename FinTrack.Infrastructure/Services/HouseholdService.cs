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

        public async Task<HouseholdDto> CreateHouseholdAsync(CreateHouseholdDto createHouseholdDto)
        {
            if (string.IsNullOrWhiteSpace(createHouseholdDto.Name))
            {
                throw new ArgumentException("Household name is required.", nameof(createHouseholdDto));
            }

            var household = new Household
            {
                Name = createHouseholdDto.Name
            };

            await _householdRepository.AddAsync(household);

            return new HouseholdDto
            {
                Id = household.Id,
                Name = household.Name,
                CreateDate = household.CreateDate
            };
        }
    }
}