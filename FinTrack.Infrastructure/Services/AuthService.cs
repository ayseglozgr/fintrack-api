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
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            // 1. Aynı email ile daha önce kayıt olunmuş mu kontrolü
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return false; // Bu email zaten kayıtlı
            }

            // 2. Yeni User nesnesini oluşturuyoruz
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                //HouseholdId = request.HouseholdId
            };

            // 3. Şifreyi güvenli bir şekilde hash'liyoruz
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            // 4. Repository üzerinden ekleme ve kaydetme işlemi
            await _userRepository.AddAsync(user);

            return true;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            // 1. Kullanıcı var mı kontrol et
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return null; // Kullanıcı bulunamadı
            }

            // 2. Şifreyi doğrula
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null; // Şifre yanlış
            }

            // 3. Şimdilik geçici bir token/başarı yanıtı dönüyoruz (JWT servisini entegre edeceğiz)
            return new LoginResponseDto
            {
                Token = "DUMMY_JWT_TOKEN", // JWT yapısını kurduğumuzda buraya gerçek token gelecek
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

    }
}