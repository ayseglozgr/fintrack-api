namespace FinTrack.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public string CreateUser { get; set; } = "System";
    public DateTime? EditDate { get; set; }
    public string? EditUser { get; set; }
    public bool IsDeleted { get; set; } = false;
}