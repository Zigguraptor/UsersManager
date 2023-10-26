using AutoMapper;
using UsersManager.Application.Common.Mappings;
using UsersManager.Domain;

namespace UsersManager.Application.Users.Queries;

public class UserInfoVm : IHaveMapping
{
    public Guid Uuid { get; set; }
    public required string UserName { get; set; }
    public required string DisplayName { get; set; }
    public required string EmailAddress { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserInfoVm>();
    }
}
