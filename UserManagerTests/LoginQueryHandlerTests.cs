using UsersManager.Application.Interfaces;
using UsersManager.Application.Users.Queries.Login;
using UsersManager.Domain;

namespace UserManagerTests;

public class LoginQueryHandlerTests
{
    [Fact]
    public void Handle_CorrectQuery_ReturnsToken()
    {
        var passwordHandlerMock = new Mock<IPasswordHandler>();
        passwordHandlerMock.Setup(handler => handler.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var tokenGeneratorMock = new Mock<ITokenGenerator>();
        tokenGeneratorMock.Setup(g => g.GenerateToken(It.IsAny<User>(), It.IsAny<bool>()))
            .Returns((User user, bool _) => user.PasswordHash);

        var usersRepositoryMock = new Mock<IUsersRepository>();
        usersRepositoryMock.Setup(r => r.GetUserAsync(It.IsAny<Guid?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new User
            {
                CreationDateTime = default,
                LastModDateTime = default,
                Uuid = default,
                IsActive = false,
                IsAdmin = false,
                UserName = "test",
                DisplayName = "test",
                EmailAddress = "test@test.com",
                PasswordHash = "qwertyuiop",
                DbDob = default,
                Dob = default
            });

        // Act
        var loginQueryHandler = new LoginQueryHandler(passwordHandlerMock.Object, tokenGeneratorMock.Object,
            usersRepositoryMock.Object);
        var result = loginQueryHandler.Handle(new LoginQuery("test", "test"), new CancellationToken(false)).Result;

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Handle_NotCorrectQuery_ReturnsNull()
    {
        var passwordHandlerMock = new Mock<IPasswordHandler>();
        passwordHandlerMock.Setup(handler => handler.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false); // false

        var tokenGeneratorMock = new Mock<ITokenGenerator>();
        tokenGeneratorMock.Setup(g => g.GenerateToken(It.IsAny<User>(), It.IsAny<bool>()))
            .Returns((User user, bool _) => user.PasswordHash);

        var usersRepositoryMock = new Mock<IUsersRepository>();
        usersRepositoryMock.Setup(r => r.GetUserAsync(It.IsAny<Guid?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new User
            {
                CreationDateTime = default,
                LastModDateTime = default,
                Uuid = default,
                IsActive = false,
                IsAdmin = false,
                UserName = "test",
                DisplayName = "test",
                EmailAddress = "test@test.com",
                PasswordHash = "qwertyuiop",
                DbDob = default,
                Dob = default
            });

        // Act
        var loginQueryHandler = new LoginQueryHandler(passwordHandlerMock.Object, tokenGeneratorMock.Object,
            usersRepositoryMock.Object);
        var result = loginQueryHandler.Handle(new LoginQuery("test", "test"), new CancellationToken(false)).Result;

        // Assert
        Assert.Null(result);
    }
}
