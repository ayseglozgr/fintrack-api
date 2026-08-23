using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(FinTrackDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> GetByAccountNameAsync(string accountName)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.AccountName.ToLower() == accountName.ToLower());
    }

    public async Task<User?> GetByAccountNameOrEmailAsync(string accountNameOrEmail)
    {
        var normalized = accountNameOrEmail.ToLower();

        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalized || u.AccountName.ToLower() == normalized);
    }

    public async Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.RefreshTokenHash == refreshTokenHash);
    }
}