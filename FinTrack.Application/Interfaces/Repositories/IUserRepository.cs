using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    // Giriş (Login) ve benzersiz e-posta kontrolü için özel metot
    Task<User?> GetByEmailAsync(string email);
}