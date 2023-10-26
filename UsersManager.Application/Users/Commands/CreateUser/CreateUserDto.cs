using System.ComponentModel.DataAnnotations;
using AutoMapper;
using UsersManager.Application.Common.Mappings;
using UsersManager.Domain;

namespace UsersManager.Application.Users.Commands.CreateUser;

public class CreateUserDto : IHaveMapping
{
    [Required(AllowEmptyStrings = false)]
    [MinLength(3)]
    public required string UserName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MinLength(1)]
    public required string DisplayName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    public required string EmailAddress { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateUserDto, User>();
    }
}
