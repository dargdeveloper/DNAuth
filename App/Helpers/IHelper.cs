using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace DotNet.Docker.Helpers;

public interface IHelper
{
    string EscapeJsonString(string json);
    JwtSecurityToken ReadJwtToken();
}