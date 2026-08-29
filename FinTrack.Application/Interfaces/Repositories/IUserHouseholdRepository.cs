using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Repositories;

public interface IUserHouseholdRepository : IGenericRepository<UserHousehold>
{
    Task<IReadOnlyList<UserHousehold>> GetByUserIdAsync(int userId);
    Task<UserHousehold?> GetByUserAndHouseholdIdAsync(int userId, int householdId);
    Task<UserHousehold?> GetActiveByUserIdAsync(int userId);
}