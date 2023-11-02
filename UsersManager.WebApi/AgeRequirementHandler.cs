using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using UsersManager.Application.Interfaces;

namespace UsersManager.WebApi;

public class AgeRequirementHandler : AuthorizationHandler<AgeRequirement>
{
    private readonly IDateTimeService _dateTimeService;

    public AgeRequirementHandler(IDateTimeService dateTimeService) => _dateTimeService = dateTimeService;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            return Task.CompletedTask;

        var value = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth)!.Value;
        if (!DateOnly.TryParse(value, out var dateOfBirth))
            return Task.CompletedTask;

        if (dateOfBirth.AddYears(requirement.Age) <= DateOnly.FromDateTime(_dateTimeService.Now))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
