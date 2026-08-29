using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByAccountNameAsync(string accountName);
    Task<User?> GetByAccountNameOrEmailAsync(string accountNameOrEmail);
    Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash);
    Task<IReadOnlyList<UserHousehold>> GetUserHouseholdsAsync(int userId);
    Task<UserHousehold?> GetUserHouseholdAsync(int userId, int householdId);
    Task<UserHousehold?> GetActiveUserHouseholdAsync(int userId);
    Task AddUserHouseholdAsync(UserHousehold userHousehold);
    Task SetActiveHouseholdAsync(int userId, int householdId);
}