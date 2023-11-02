using Microsoft.AspNetCore.Authorization;

namespace UsersManager.WebApi;

public class AgeRequirement : IAuthorizationRequirement
{
    public int Age { get; }
    public AgeRequirement(int age) => Age = age;
}
