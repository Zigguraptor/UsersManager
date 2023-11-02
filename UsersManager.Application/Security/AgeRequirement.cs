using Microsoft.AspNetCore.Authorization;

namespace UsersManager.Application.Security;

public class AgeRequirement : IAuthorizationRequirement
{
    public int Age { get; }
    public AgeRequirement(int age) => Age = age;
}
