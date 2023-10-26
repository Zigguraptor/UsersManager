using MediatR;

namespace UsersManager.Application.Users.Queries;

public class UserInfoQuery : IRequest<UserInfoVm?>
{
    public Guid? Guid { get; set; }
    public string? UserName { get; set; }
    public string? EmailAddress { get; set; }
}
