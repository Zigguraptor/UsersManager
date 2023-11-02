using System.Text.Json.Serialization;
using AutoMapper;
using UsersManager.Application.Common.Mappings;
using UsersManager.Domain;

namespace UsersManager.Application.Users.Queries.UserInfo;

public class UserInfoVm : IHaveMapping
{
    public Guid Uuid { get; set; }
    public required string UserName { get; set; }
    public required string DisplayName { get; set; }
    public required string EmailAddress { get; set; }

    [JsonPropertyName("Date of Birth")] public required DateOnly Dob { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserInfoVm>();
    }
}
