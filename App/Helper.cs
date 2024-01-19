using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DotNet.Docker;

public class Helper
{
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
    
    public static string EscapeJsonString(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return json;
        }

        int firstQuoteIndex = json.IndexOf('"');
        int lastQuoteIndex = json.LastIndexOf('"');

        if (firstQuoteIndex == -1 || lastQuoteIndex == -1 || firstQuoteIndex == lastQuoteIndex)
        {
            // There are no quotes, or only one quote in the string.
            return json;
        }

        string beforeFirstQuote = json.Substring(0, firstQuoteIndex + 1);
        string afterLastQuote = json.Substring(lastQuoteIndex);
        string middlePart = json.Substring(firstQuoteIndex + 1, lastQuoteIndex - firstQuoteIndex - 1)
            /*.Replace("\"", "\"\"")*/;

        return middlePart;
    }
}