using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.DTOs.Household
{
    public class UpdateHouseholdDto
    {
        [Required]
        public string Uid { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
