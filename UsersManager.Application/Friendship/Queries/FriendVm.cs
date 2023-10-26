using AutoMapper;
using UsersManager.Application.Common.Mappings;
using UsersManager.Domain;

namespace UsersManager.Application.Friendship.Queries;

public class FriendVm : IHaveMapping
{
    public required string UserName { get; set; }
    public required string DisplayName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, FriendVm>();
    }
}
