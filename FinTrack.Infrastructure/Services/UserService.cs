using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;

namespace FinTrack.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user != null;
    }

    public async Task<bool> AssignHouseholdAsync(int userId, int householdId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        user.HouseholdId = householdId;
        await _userRepository.UpdateAsync(user);

        return true;
    }
}
