using DotNet.Docker.Models;

namespace DotNet.Docker.HttpServices;

public interface IHttpService
{
    Task<string> GetAsync(string url);
    Task<string> EnviarFormularioAsync(TokenModel data);
    Task<string> GetList(string bearerToken);
}