using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByAccountNameAsync(string accountName);
    Task<User?> GetByAccountNameOrEmailAsync(string accountNameOrEmail);
    Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash);
}