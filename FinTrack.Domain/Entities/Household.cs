namespace FinTrack.Domain.Entities;

public class Household : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<UserHousehold> UserHouseholds { get; set; } = new List<UserHousehold>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}