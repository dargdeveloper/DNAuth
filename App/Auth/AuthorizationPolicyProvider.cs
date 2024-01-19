using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace DotNet.Docker.Auth;

public class AuthorizationPolicyProvider: DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;
    private string BaseUri { get; }

    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options,Uri authorizationUrl) : base(options)
    {
        _options = options.Value;
            
        //var authorizationUrl = new Uri(Environment.GetEnvironmentVariable("AUTHORIZATION_URL"));
        BaseUri = authorizationUrl.GetLeftPart(UriPartial.Authority);
    }

    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        // Check static policies first
        var policy = await base.GetPolicyAsync(policyName);
        
        if (policy == null)
        { 
          
            policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new HasScopeRequirement(policyName, $"{BaseUri}/"))
                .Build();

            // Add policy to the AuthorizationOptions, so we don't have to re-create it each time
            _options.AddPolicy(policyName, policy);
        }

        return policy;
    }
}