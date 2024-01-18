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
}