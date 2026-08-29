using System.ComponentModel.DataAnnotations;
using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Application.DTOs.Category;

public class CreateCategoryRequestDto
{
    public string? HouseholdUid { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? ParentCategoryUid { get; set; }

    [Range(1, 2)]
    public LedgerDirection Direction { get; set; }

    public bool IsActive { get; set; } = true;
}