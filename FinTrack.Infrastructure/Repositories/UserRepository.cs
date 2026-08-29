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
        return await _context.User
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> GetByAccountNameAsync(string accountName)
    {
        return await _context.User
            .FirstOrDefaultAsync(u => u.AccountName.ToLower() == accountName.ToLower());
    }

    public async Task<User?> GetByAccountNameOrEmailAsync(string accountNameOrEmail)
    {
        var normalized = accountNameOrEmail.ToLower();

        return await _context.User
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalized || u.AccountName.ToLower() == normalized);
    }

    public async Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash)
    {
        return await _context.User
            .FirstOrDefaultAsync(u => u.RefreshTokenHash == refreshTokenHash);
    }

    public async Task<IReadOnlyList<UserHousehold>> GetUserHouseholdsAsync(int userId)
    {
        return await _context.UserHousehold
            .Include(uh => uh.Household)
            .Where(uh => uh.UserId == userId)
            .OrderByDescending(uh => uh.IsActive)
            .ThenBy(uh => uh.JoinedAt)
            .ToListAsync();
    }

    public async Task<UserHousehold?> GetUserHouseholdAsync(int userId, int householdId)
    {
        return await _context.UserHousehold
            .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.HouseholdId == householdId);
    }

    public async Task<UserHousehold?> GetActiveUserHouseholdAsync(int userId)
    {
        return await _context.UserHousehold
            .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.IsActive);
    }

    public async Task AddUserHouseholdAsync(UserHousehold userHousehold)
    {
        await _context.UserHousehold.AddAsync(userHousehold);
        await _context.SaveChangesAsync();
    }

    public async Task SetActiveHouseholdAsync(int userId, int householdId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var memberships = await _context.UserHousehold
            .Where(uh => uh.UserId == userId)
            .ToListAsync();

        foreach (var membership in memberships)
        {
            membership.IsActive = membership.HouseholdId == householdId;
        }

        _context.UserHousehold.UpdateRange(memberships);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}