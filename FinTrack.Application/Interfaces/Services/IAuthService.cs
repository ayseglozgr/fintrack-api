using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.User;

namespace FinTrack.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<LoginResponseDto>> RegisterAsync(RegisterRequestDto request);
        Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
        Task<ServiceResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
