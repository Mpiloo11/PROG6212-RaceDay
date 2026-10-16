using RaceDay.API;
using Xunit;

namespace RaceDay.Tests;

public class PasswordSecurityTests
{
    [Fact]
    public void HashPassword_DoesNotReturnOriginalPassword()
    {
        var password = "TestPass123!";

        var hash = PasswordSecurity.HashVersioned(password);

        Assert.NotEqual(password, hash);
        Assert.False(string.IsNullOrWhiteSpace(hash));
    }

    [Fact]
    public void HashPassword_CreatesDifferentHashesForSamePassword()
    {
        var password = "TestPass123!";

        var firstHash = PasswordSecurity.HashVersioned(password);
        var secondHash = PasswordSecurity.HashVersioned(password);

        Assert.NotEqual(firstHash, secondHash);
    }

    [Fact]
    public void VerifyVersioned_AcceptsCorrectPassword()
    {
        var password = "TestPass123!";
        var hash = PasswordSecurity.HashVersioned(password);

        var result = PasswordSecurity.VerifyVersioned(password, hash);

        Assert.True(result);
    }

    [Fact]
    public void VerifyVersioned_RejectsIncorrectPassword()
    {
        var password = "TestPass123!";
        var hash = PasswordSecurity.HashVersioned(password);

        var result = PasswordSecurity.VerifyVersioned(
            "WrongPassword123!",
            hash);

        Assert.False(result);
    }
}