using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DotNet.Docker.Helpers;

public class Helper:IHelper
{
    private readonly HttpContext context;
    
    public Helper(
        IHttpContextAccessor httpContextAccessor
    )
    {
        context = httpContextAccessor.HttpContext;
    }
    
    // Función para leer y convertir la clave pública PEM a RsaSecurityKey
    public static RsaSecurityKey GetPublicKey(string publicKeyPem)
    {
        var publicKeyBytes = Encoding.ASCII.GetBytes(publicKeyPem);
        // Convertir byte array a string
        string pemString = Encoding.UTF8.GetString(publicKeyBytes);
        var rsa = RSA.Create();
        rsa.ImportFromPem(pemString);
        return new RsaSecurityKey(rsa);
    }
    
    public string EscapeJsonString(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return json;
        }

        int firstQuoteIndex = json.IndexOf('"');
        int lastQuoteIndex = json.LastIndexOf('"');

        if (firstQuoteIndex == -1 || lastQuoteIndex == -1 || firstQuoteIndex == lastQuoteIndex)
        {
            return json;
        }

        string middlePart = json.Substring(firstQuoteIndex + 1, lastQuoteIndex - firstQuoteIndex - 1);

        return middlePart;
    }
    
    public JwtSecurityToken ReadJwtToken()
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
        {
            return null;
        }

        var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
    
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        return jwtToken;
    }
}