namespace FinTrack.Application.DTOs.Category;

public class CategoryGetDto
{
    public string Uid { get; set; } = string.Empty;
    public string? HouseholdUid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ParentCategoryUid { get; set; }
    public int Direction { get; set; }
    public bool IsActive { get; set; }
}