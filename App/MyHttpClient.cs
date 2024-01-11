using System.Net.Http;
using System.Threading.Tasks;


public class MyHttpClient
{
    private readonly HttpClient _httpClient;

    public MyHttpClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetAsync(string url)
    {
        try
        {
            Console.WriteLine(url);
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            // Aquí puedes manejar los errores de la solicitud
            Console.WriteLine($"Error en la solicitud: {e.Message}");
            return "Error";
        }
    }
}
