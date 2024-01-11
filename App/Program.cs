// using Microsoft.OpenApi.Models;

// var builder = WebApplication.CreateBuilder(args);

// // Agrega servicios a la aplicación
// builder.Services.AddControllers();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });
// });

// var app = builder.Build();

// // Configura el HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error");
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();
// app.UseRouting();

// app.UseAuthorization();

// app.MapControllers();

// app.UseSwagger();
// app.UseSwaggerUI(/*c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
// }*/);

// var beers = new Beer[]
// {
// new Beer("x","y"),
// new Beer("z","a"),
// new Beer("b","c"),
// };

// app.MapGet("/beers/{quantity}", ()=>{
//     return beers;
// });

// app.MapGet("/", () => "Hello World!");


// app.Run();


// internal record Beer(string name, string brand);

using System.Text.Json;
using DotNet.Docker;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddHttpClient("FormClient", client =>
// {
//     client.BaseAddress = new Uri("http://tuapi.com/");
//     client.DefaultRequestHeaders.Add("Accept", "application/json");
// });

builder.Services.AddAuthentication()
    .AddOAuth("sso", o =>
    {
        o.ClientId = "9b0e5e01-1474-445e-8496-627d08c61656";
        o.ClientSecret = "9Tq3EHiuIakfAClpU3Wgm6X0EvVTlzh0ZXJNIOPS";
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Registro del servicio HTTP
builder.Services.AddScoped<IHttpService, HttpService>();
// Agrega esta línea para registrar HttpClient
builder.Services.AddHttpClient();
// Add services to the container.
builder.Logging.AddConsole();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.MapGet("/", () => "Hello, World!");

var beers = new Beer[]
{
new Beer("x","y"),
new Beer("z","a"),
new Beer("b","c"),
};

app.MapGet("/beers/{quantity}", (int quantity) =>
{
    return beers.Take(quantity);
}).AddEndpointFilter(async (context, next) =>
{
    int quantity = context.GetArgument<int>(0);

    if (quantity <= 0)
    {
        return Results.Problem("Favor enviar un valor mayor a cero (0).");
    }

    return await next(context);
});

app.MapGet("/external-data", async (IHttpService httpService) => 
{
    try
    {
        string url = "https://newsapi.org/v2/everything?q=tesla&from=2023-12-09&sortBy=publishedAt&apiKey=232a36396aef4d618886549ee8511220";
        var result = await httpService.GetAsync(url);
        // Console.WriteLine(result);
        // return next(result);
        var myObject = JsonSerializer.Deserialize<Root>(result);
        return Results.Ok(myObject);
        // return result;
    }
    catch (Exception e)
    {
        return Results.Problem("Error interno del servidor" + e.Message);
    }
});

app.MapGet("/external-api", async (IHttpClientFactory clientFactory, ILogger<Program> logger) =>
{
    var client = clientFactory.CreateClient();
    client.DefaultRequestHeaders.UserAgent.ParseAdd("YourAppName/1.0");
    try
    {
        var response = await client.GetAsync("https://newsapi.org/v2/everything?q=tesla&from=2023-12-09&sortBy=publishedAt&apiKey=232a36396aef4d618886549ee8511220");
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            logger.LogError($"API request failed: {response.StatusCode}. Response content: {errorContent}");
            return Results.Problem($"API request failed: {response.StatusCode}");
        }

        var responseData = await response.Content.ReadAsStringAsync();
        var myObject = JsonSerializer.Deserialize<Root>(responseData);
        return Results.Ok(myObject);
    }
    catch (HttpRequestException ex)
    {
        logger.LogError($"HttpRequestException: {ex.Message}");
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/enviar-formulario", async (TokenModel data, IHttpService httpService) =>
{
    try
    {
        var resultado = await httpService.EnviarFormularioAsync(data);
        // return Results.Ok(resultado);
        var myObject = JsonSerializer.Deserialize<TokenResponse>(resultado);
        return Results.Ok(myObject);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();

internal record Beer(string name, string brand);

public class Article
{
    public Source source { get; set; }
    public string author { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string url { get; set; }
    public string urlToImage { get; set; }
    public DateTime publishedAt { get; set; }
    public string content { get; set; }
}

public class Root
{
    public string status { get; set; }
    public int totalResults { get; set; }
    public List<Article> articles { get; set; }
}

public class Source
{
    public string id { get; set; }
    public string name { get; set; }
}