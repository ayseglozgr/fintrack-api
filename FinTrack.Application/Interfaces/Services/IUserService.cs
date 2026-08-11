namespace FinTrack.Application.Interfaces.Services;

public interface IUserService
{
    Task<bool> UserExistsAsync(int userId);
    Task<bool> AssignHouseholdAsync(int userId, int householdId);
}
