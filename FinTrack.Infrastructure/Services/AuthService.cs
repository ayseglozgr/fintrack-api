using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.User;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FinTrack.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthService(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ServiceResponse<LoginResponseDto>> RegisterAsync(RegisterRequestDto request)
        {
            string normalizedEmail = request.Email.Trim().ToLowerInvariant();
            string normalizedAccountName = request.AccountName.Trim().ToLowerInvariant();

            var existingUserByEmail = await _userRepository.GetByEmailAsync(normalizedEmail);
            if (existingUserByEmail != null)
            {
                return ServiceResponse<LoginResponseDto>.Failure("Email is already in use.");
            }

            var existingUserByAccountName = await _userRepository.GetByAccountNameAsync(normalizedAccountName);
            if (existingUserByAccountName != null)
            {
                return ServiceResponse<LoginResponseDto>.Failure("AccountName is already in use.");
            }

            var user = new User
            {
                AccountName = request.AccountName.Trim(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = normalizedEmail
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _userRepository.AddAsync(user);

            var tokenResponse = await CreateAndPersistTokensAsync(user);
            return ServiceResponse<LoginResponseDto>.Success(tokenResponse, "Registration completed successfully.");
        }

        public async Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByAccountNameOrEmailAsync(request.AccountNameOrEmail.Trim());
            if (user == null)
            {
                return ServiceResponse<LoginResponseDto>.Failure("Invalid credentials.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return ServiceResponse<LoginResponseDto>.Failure("Invalid credentials.");
            }

            var tokenResponse = await CreateAndPersistTokensAsync(user);
            return ServiceResponse<LoginResponseDto>.Success(tokenResponse, "Login successful.");
        }

        public async Task<ServiceResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return ServiceResponse<LoginResponseDto>.Failure("Refresh token is required.");
            }

            string tokenHash = _jwtTokenService.HashRefreshToken(request.RefreshToken.Trim());
            var user = await _userRepository.GetByRefreshTokenHashAsync(tokenHash);

            if (user == null || !user.RefreshTokenExpiresAtUtc.HasValue || user.RefreshTokenExpiresAtUtc.Value <= DateTime.UtcNow)
            {
                return ServiceResponse<LoginResponseDto>.Failure("Invalid or expired refresh token.");
            }

            var tokenResponse = await CreateAndPersistTokensAsync(user);
            return ServiceResponse<LoginResponseDto>.Success(tokenResponse, "Token refreshed successfully.");
        }

        private async Task<LoginResponseDto> CreateAndPersistTokensAsync(User user)
        {
            var accessTokenExpiresAtUtc = _jwtTokenService.GetAccessTokenExpiresAtUtc();
            var accessToken = _jwtTokenService.GenerateAccessToken(user, accessTokenExpiresAtUtc);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var refreshTokenHash = _jwtTokenService.HashRefreshToken(refreshToken);
            var refreshTokenExpiresAtUtc = _jwtTokenService.GetRefreshTokenExpiresAtUtc();

            user.RefreshTokenHash = refreshTokenHash;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpiresAtUtc;
            await _userRepository.UpdateAsync(user);

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                AccountName = user.AccountName,
                Email = user.Email,
                AccessToken = accessToken,
                AccessTokenExpiresAtUtc = accessTokenExpiresAtUtc,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAtUtc = refreshTokenExpiresAtUtc
            };

            return response;
        }

    }
}