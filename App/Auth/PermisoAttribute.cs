using Microsoft.AspNetCore.Authorization;

namespace DotNet.Docker.Auth;

[AttributeUsage(AttributeTargets.All)]
public class PermisoAttribute: AuthorizeAttribute
{
    public PermisoAttribute(object access,string schema=null)
    {
        if (schema != null)
        {
            AuthenticationSchemes = schema;
        }

        
        Policy = access.ToString();
        
    }
}