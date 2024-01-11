using System.Net.Http.Headers;

namespace DotNet.Docker;

public class HttpService: IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient client;
    private readonly ILogger<HttpService> logger;
    // private readonly var formClient;

    public HttpService(
        HttpClient httpClient,
        IHttpClientFactory clientFactory,
        ILogger<HttpService> log
        )
    {
        _httpClient = httpClient;
        client = clientFactory.CreateClient();
        logger = log;
        // formClient = clientFactory.CreateClient("FormClient");
    }

    public async Task<string> GetAsync(string url)
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("YourAppName/1.0");
        // _httpClient.DefaultRequestHeaders.Accept.Clear();
        // _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        // _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("YourAppName/1.0");

        HttpResponseMessage response = await client.GetAsync(url);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            logger.LogError($"API request failed: {response.StatusCode}. Response content: {errorContent}");
            // return Results.Problem($"API request failed: {response.StatusCode}");
        }
        // response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> EnviarFormularioAsync(TokenModel data)
    {
        // var client = _clientFactory.CreateClient("FormClient");

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", data.ClientId),
            new KeyValuePair<string, string>("client_secret", data.ClientSecret),
            new KeyValuePair<string, string>("grant_type", data.GrantType),
            new KeyValuePair<string, string>("username", data.Username),
            new KeyValuePair<string, string>("password", data.Password),
            new KeyValuePair<string, string>("scope", data.Scope),
        });

        // logger.LogError("Llego hasta antes de la peticion");
        var response = await client.PostAsync("http://172.28.0.4/oauth/token", content);

        // logger.LogError("Llego hasta despues de la peticion");
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            logger.LogError($"API request failed: {response.StatusCode}. Response content: {errorContent}");
            // return Results.Problem($"API request failed: {response.StatusCode}");
        }
        
        // logger.LogError("Llego hasta despues del if del error");
        
        // if (response.IsSuccessStatusCode)
        // {
            return await response.Content.ReadAsStringAsync();
        // }

        // throw new Exception("Error al enviar el formulario");
    }
}