using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.DTOs.Household
{
    public class UpdateHouseholdDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
