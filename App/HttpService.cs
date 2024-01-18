using System.Net.Http.Headers;

namespace DotNet.Docker;

public class HttpService: IHttpService
{
    private readonly HttpClient client;
    private readonly ILogger<HttpService> logger;
    // private readonly var formClient;

    public HttpService(
        IHttpClientFactory clientFactory,
        ILogger<HttpService> log
        )
    {
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
        var response = await client.PostAsync("http://localhost:8082/oauth/token", content);

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

    public async Task<string> GetList(string bearerToken)
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI5YjFiNzQ0MS01MmFhLTQ3MDEtYjRlMi05OTk3MWJiMWMwNmIiLCJqdGkiOiIxODU5M2E4NmNlMDNhNzNlYWZjN2Q1YjYxMjlmMTNjMThhYjE0ZTMzMmRlYTY3OTQzMWFkYjQ2MzFiOWZmNmEwMjIwMDE0OWIwMGZmYjdiOSIsImlhdCI6MTcwNTQ2NzI4My4wNjY3MDMsIm5iZiI6MTcwNTQ2NzI4My4wNjY3MDYsImV4cCI6MTcwNTQ2NzU4My4wNDk5NSwic3ViIjoiMTAyOTEwZTMtNjdjMy00YmRmLWIyODYtYzlkMzNmMDY1Zjk0Iiwic2NvcGVzIjpbXX0.QQvsUZjieckborsG1-OROyTFH8xxNLvaVtM-dSnlHefbuyZQIP4oE4qY_x1VOqzPTAF10h93XNDuPl23BWUtGe0eay4dLxd5LajyAdXn1lJMGrsZuPtMT0fuvHmcYJxKySZMrjTD7wlRGwH5DRmf3uz9Gr-BHpSWyI3C6DatjO4w5DXyXCq4JUqIWoFvZGrNX2ECAzyA1uKPgJFxc_yp0Y3y_1TdMEe__i4DLthgFFwHRIlkGLqKhHo6BCDfjwIm6NzygarDhlgTWTIRoaidt6gzAU3cVGxToe7mDgKMuLWYJZTTys_NAiJCIUbTLvm2RQn3PYgvxM0laW0o9ZNougeXe-gJyHwUMCvYxzv4ByADlwcMxHTtLudrOWcP6H6UKqzVUS--dsZMCXqwslHcZU8dx_K9aKmY4Dhbtvq6V8-z3z5VKDa7s3ukMES0Wmw5QGEqpo2rrLsrq66IUJvzvsDqHoLS6pWZqbRp5qQC8KInpDRvAuAhKHqhXvejAZTyjxQAfkeis-pvDaLp5OJrq2o5WnAVvou4goMNLu4VmsaoUnlkfKx6L76d_pXvn9wDxpbSp8S5XIG3l2z1aAoo-6Fe8q2sCgS1dAU1_UurHUnmxVRIw73BXtYiKSGcFPOepSq6W5oxLvwvQlBZaCiOvTmXl943IP7Kv4wdHaUmmWY");
        // var response = await client.GetAsync("https://dev-sso.proyectos-enee.xyz/api/v1/catalogos/list?with[]=padres&tipo[]=proyecto");
        var response = await client.GetAsync("http://localhost:8082/api/v1/catalogos/list?with[]=padres&tipo[]=proyecto");

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
    }
}