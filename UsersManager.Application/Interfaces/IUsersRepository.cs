using UsersManager.Domain;

namespace UsersManager.Application.Interfaces;

public interface IUsersRepository
{
    public Task<User?> GetUserAsync(Guid? userGuid = null, string? userName = null, string? emailAddress = null);
    public Task<Guid> InsertUserAsync(User user);
    public Task<int> UpdateUserAsync(User user);
    public Task<int> DeactivateUserAsync(Guid userGuid);
}
