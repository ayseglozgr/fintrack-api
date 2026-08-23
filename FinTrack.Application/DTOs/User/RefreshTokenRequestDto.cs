using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.DTOs.User
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
