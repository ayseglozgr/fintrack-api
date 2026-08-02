using FinTrack.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrack.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDto request);
    }
}
