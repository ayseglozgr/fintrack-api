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

        [Required]
        public string UserUid { get; set; } = string.Empty; // Haneyi oluşturan kullanıcının şifreli ID'si
    }
}
