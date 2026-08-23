namespace FinTrack.Domain.Entities;

public class User : BaseEntity
{
    public string AccountName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // Güvenlik için şifreyi açık tutmayacağız
    public string? RefreshTokenHash { get; set; }
    public DateTime? RefreshTokenExpiresAtUtc { get; set; }
    public int? HouseholdId { get; set; }
    public Household Household { get; set; } = null!;
}