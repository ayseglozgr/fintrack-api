using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrack.Application.DTOs.User
{
    public class RegisterRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        //public int HouseholdId { get; set; }
    }
}
