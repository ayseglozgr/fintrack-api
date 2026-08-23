using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.DTOs.User
{
    public class LoginRequestDto
    {
        [Required]
        [MaxLength(256)]
        public string AccountNameOrEmail { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;
    }
}
