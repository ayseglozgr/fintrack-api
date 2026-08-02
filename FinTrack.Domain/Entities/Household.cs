namespace FinTrack.Domain.Entities;

public class Household : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<User> User { get; set; } = new List<User>();
}