using Microsoft.AspNetCore.Authorization;

namespace DotNet.Docker.Auth;

public class HasScopeHandler: AuthorizationHandler<HasScopeRequirement>
{
    // public IRepository<Usuarios> Repository { get; }
    //
    // public HasScopeHandler(IRepository<Usuarios> repository)
    // {
    //     Repository = repository;
    // }
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasScopeRequirement requirement
    )
    {
        var userName = context.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        if (
            !string.IsNullOrWhiteSpace(userName)
            && userName == Environment.GetEnvironmentVariable("USERNAME_ADMIN")
        )
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (!context.User.HasClaim(x => x.Type == "permissions" && requirement.Issuer == x.Issuer))
            return Task.CompletedTask;

        var scopes = context.User.Claims
            .Where(x => x.Type == "permissions")
            .Select(x => x.Value)
            .ToList();

        if (scopes.Any(s => s == requirement.Scope))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}