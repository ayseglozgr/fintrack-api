using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Repositories;

public class UserHouseholdRepository : GenericRepository<UserHousehold>, IUserHouseholdRepository
{
    public UserHouseholdRepository(FinTrackDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<UserHousehold>> GetByUserIdAsync(int userId)
    {
        return await _context.UserHousehold
            .Include(uh => uh.Household)
            .Where(uh => uh.UserId == userId)
            .OrderByDescending(uh => uh.IsActive)
            .ThenBy(uh => uh.JoinedAt)
            .ToListAsync();
    }

    public async Task<UserHousehold?> GetByUserAndHouseholdIdAsync(int userId, int householdId)
    {
        return await _context.UserHousehold
            .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.HouseholdId == householdId);
    }

    public async Task<UserHousehold?> GetActiveByUserIdAsync(int userId)
    {
        return await _context.UserHousehold
            .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.IsActive);
    }
}