using Microsoft.AspNetCore.Identity;

namespace RaceDay.API;

public static class PasswordSecurity
{
    private static readonly PasswordHasher<string> Hasher = new();

    public static string HashVersioned(string password)
    {
        return Hasher.HashPassword("RaceDay", password);
    }

    public static bool VerifyVersioned(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            return false;
        }

        var result = Hasher.VerifyHashedPassword(
            "RaceDay",
            hashedPassword,
            password);

        return result != PasswordVerificationResult.Failed;
    }
}