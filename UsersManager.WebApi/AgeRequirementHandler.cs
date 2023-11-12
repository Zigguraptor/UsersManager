using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using UsersManager.Application.Interfaces;

namespace UsersManager.WebApi;

public class AgeRequirementHandler : AuthorizationHandler<AgeRequirement>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<AgeRequirement> _logger;

    public AgeRequirementHandler(IDateTimeService dateTimeService, ILogger<AgeRequirement> logger)
    {
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
        {
            _logger.LogDebug("User has no claim DateOfBirth. userUuid:{uuid}", context.User.Identity?.Name);
            return Task.CompletedTask;
        }

        var value = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth)!.Value;
        if (!DateOnly.TryParse(value, out var dateOfBirth))
        {
            _logger.LogError("DateOfBirth parsing error value:{value}; userUuid:{uuid};", value,
                context.User.Identity?.Name);
            return Task.CompletedTask;
        }

        if (dateOfBirth.AddYears(requirement.Age) <= DateOnly.FromDateTime(_dateTimeService.Now))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
