using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinTrack.Application.DTOs.Household
{
    public class CreateHouseholdDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int UserId { get; set; } // Haneyi oluşturan kullanıcının ID'si
    }
}
