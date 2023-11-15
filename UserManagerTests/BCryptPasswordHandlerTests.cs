using UsersManager.Application.Security;

namespace UserManagerTests;

public class BCryptPasswordHandlerTests
{
    [Fact]
    public void ValidatePassword_DifferentPasswords_ReturnsFalse()
    {
        var bCryptPasswordHandler = new BCryptPasswordHandler();
        const string pass1 = "qwerty";
        const string pass2 = "ytrewq";
        var passHash1 = bCryptPasswordHandler.HashPassword(pass1);
        var passHash2 = bCryptPasswordHandler.HashPassword(pass2);

        // Act
        var validateResult1 = bCryptPasswordHandler.ValidatePassword(pass1, passHash2);
        var validateResult2 = bCryptPasswordHandler.ValidatePassword(pass2, passHash1);

        // Assert
        Assert.False(validateResult1);
        Assert.False(validateResult2);
    }

    [Fact]
    public void ValidatePassword_CurrentPasswords_ReturnsTrue()
    {
        var bCryptPasswordHandler = new BCryptPasswordHandler();
        const string pass1 = "qwerty";
        const string pass2 = "ytrewq";
        var passHash1 = bCryptPasswordHandler.HashPassword(pass1);
        var passHash2 = bCryptPasswordHandler.HashPassword(pass2);

        // Act
        var validateResult1 = bCryptPasswordHandler.ValidatePassword(pass1, passHash1);
        var validateResult2 = bCryptPasswordHandler.ValidatePassword(pass2, passHash2);

        // Assert
        Assert.True(validateResult1);
        Assert.True(validateResult2);
    }
}
