using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace DotNet.Docker.Auth;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, string authorizationUrl)
    {
        services.AddSingleton<IAuthorizationPolicyProvider>(provider =>
            new AuthorizationPolicyProvider(provider.GetService<IOptions<AuthorizationOptions>>(),
                new Uri(authorizationUrl)));
        // services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        return services;
    }
}