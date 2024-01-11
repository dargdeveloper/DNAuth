namespace DotNet.Docker;

public interface IHttpService
{
    Task<string> GetAsync(string url);
    Task<string> EnviarFormularioAsync(TokenModel data);
}