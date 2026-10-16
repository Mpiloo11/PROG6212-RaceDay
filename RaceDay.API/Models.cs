
using System.ComponentModel.DataAnnotations;
namespace RaceDay.Api;

public sealed class UserAccount
{
    public int Id { get; set; }
    [Required, MaxLength(120)] public string FullName { get; set; } = "";
    [Required, MaxLength(256)] public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required, MaxLength(20)] public string Role { get; set; } = "Participant";
    [MaxLength(30)] public string? Phone { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
public sealed class EventType { public int Id { get; set; } [Required, MaxLength(30)] public string Name { get; set; } = ""; public List<RaceEvent> Events { get; set; } = []; }
public sealed class RaceEvent
{
    public int Id { get; set; }
    [Required, MaxLength(120)] public string Name { get; set; } = "";
    [MaxLength(1000)] public string Description { get; set; } = "";
    public DateTime EventDate { get; set; }
    [Required, MaxLength(200)] public string Location { get; set; } = "";
    public decimal DistanceKm { get; set; }
    public int EventTypeId { get; set; }
    public EventType? EventType { get; set; }
    public int OrganiserId { get; set; }
    public UserAccount? Organiser { get; set; }
    public List<Category> Categories { get; set; } = [];
}
public sealed class Category
{
    public int Id { get; set; }
    [Required, MaxLength(80)] public string Name { get; set; } = "";
    [MaxLength(200)] public string? Description { get; set; }
    public int RaceEventId { get; set; }
    public RaceEvent? RaceEvent { get; set; }
}
public sealed class Enrolment
{
    public int Id { get; set; }
    public int ParticipantId { get; set; }
    public UserAccount? Participant { get; set; }
    public int RaceEventId { get; set; }
    public RaceEvent? RaceEvent { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    [Required, MaxLength(20)] public string Status { get; set; } = "Confirmed";
    public DateTime EnrolledUtc { get; set; } = DateTime.UtcNow;
}
public sealed class RaceResult
{
    public int Id { get; set; }
    public int EnrolmentId { get; set; }
    public Enrolment? Enrolment { get; set; }
    public TimeSpan FinishTime { get; set; }
    public int Position { get; set; }
    public DateTime RecordedUtc { get; set; } = DateTime.UtcNow;
}
public sealed record RegisterRequest(string FullName, string Email, string Password, string Role, string? Phone);
public sealed record LoginRequest(string Email, string Password);
public sealed record EventRequest(string Name, string Description, DateTime EventDate, string Location, decimal DistanceKm, int EventTypeId);
public sealed record CategoryRequest(string Name, string? Description, int RaceEventId);
public sealed record EnrolRequest(int RaceEventId, int CategoryId);
public sealed record ResultRequest(int EnrolmentId, TimeSpan FinishTime, int Position);
public static class PasswordSecurity
{
    public static string Hash(string password) => HashVersioned(password);
    public static bool Verify(string password, string encoded) => VerifyVersioned(password, encoded);
    public static string HashVersioned(string password)
    {
        var salt = System.Security.Cryptography.RandomNumberGenerator.GetBytes(16);
        var hash = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(password, salt, 120_000, System.Security.Cryptography.HashAlgorithmName.SHA256, 32);
        return $"pbkdf2-sha256$120000${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }
    public static bool VerifyVersioned(string password, string encoded)
    {
        var parts = encoded.Split('$'); if (parts.Length != 4 || parts[0] != "pbkdf2-sha256" || !int.TryParse(parts[1], out var iterations)) return false;
        try { var salt = Convert.FromBase64String(parts[2]); var expected = Convert.FromBase64String(parts[3]); var actual = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, System.Security.Cryptography.HashAlgorithmName.SHA256, expected.Length); return System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(actual, expected); } catch { return false; }
    }
}
