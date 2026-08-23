using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user, DateTime expiresAtUtc);
    DateTime GetAccessTokenExpiresAtUtc();
    string GenerateRefreshToken();
    string HashRefreshToken(string refreshToken);
    DateTime GetRefreshTokenExpiresAtUtc();
}
