using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;
using DotNet.Docker;
using DotNet.Docker.Auth;
using DotNet.Docker.CustomAuth;
using DotNet.Docker.Redis;
using DotNet.Docker.SSO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// En tu configuración de servicios
// var publicKey = File.ReadAllText("./clave.pem");
// var rsaSecurityKey = Helper.GetPublicKey(publicKey);

//Add All Depended Services Here 
// builder.Services.AddTransient<CustomJwtBearerHandler>();
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddScheme<JwtBearerOptions, CustomJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, options => { });

builder.Services
    // .AddCustomAuthorization("https://dev-sso.proyectos-enee.xyz")
    .AddCustomAuthorization("http://172.28.0.5:4000")
    /*.AddAuthorization(option =>
    {
        option.AddPolicy("custom", AuthorizationExtensions.AddCustomAuthorization("https://dev-sso.proyectos-enee.xyz"));
    })*/
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // options.Authority = "https://dev-sso.proyectos-enee.xyz";
        // options.Audience = "9b1b7441-52aa-4701-b4e2-99971bb1c06b";
        
        options.Authority = "http://172.28.0.5:8082";
        // options.Audience = "9ae255e8-6264-4a58-89a2-eca665f107a5";
        options.Audience = "99bffb77-8e2a-4728-8a6f-72cee9e2b8f5";
        
        var publicKey = File.ReadAllText("./clave-local.pem");
        var tokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "preferred_username",
            ValidateIssuer = false,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            // ValidIssuer ="https://dev-sso.proyectos-enee.xyz",
            ValidIssuer = "http://172.28.0.5:8082",
            ValidAudience = options.Audience, // El mismo value que especificaste en el cliente de Angular
            IssuerSigningKey = Helper.GetPublicKey(publicKey)

        };
        options.TokenValidationParameters = tokenValidationParameters;
        options.RequireHttpsMetadata = false;
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = async context =>
            {
                var x = context.Result;
            }
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    // option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    // {
    //     In = ParameterLocation.Header,
    //     name = "Authorization",
    //     Type = SecuritySchemeType.ApiKey
    // });
    // option.OperationFilter<SecurityRequirementsOperationFilter>();
});
// Registro del servicio HTTP
builder.Services.AddScoped<IHttpService, HttpService>();
// Agrega esta línea para registrar HttpClient
builder.Services.AddHttpClient();
// Add services to the container.
builder.Logging.AddConsole();

// Agregar servicios de autorización
builder.Services.AddAuthorization();

// Leer la configuración de Redis
var redisConfig = builder.Configuration.GetSection("Redis").Get<MyRedisConfig>();
// Establecer y registrar la conexión de Redis
var multiplexer = ConnectionMultiplexer.Connect(redisConfig.ConnectionString);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

// Registrar el servicio de Redis
builder.Services.AddScoped<IRedisService, RedisService>();

var app = builder.Build();

// Usa la autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.MapPost("/token",async (TokenModel data, IHttpService httpService, HttpContext context) =>
{
    try
    {
        var x = context.User.Claims.ToList();
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

app.MapGet("/get-list",  [Authorize] async (IHttpService httpService, HttpContext context) =>
{
    try
    {
        var y = multiplexer.GetDatabase();
        var x = context.User.Claims.ToList();
        // Get the token from the Authorization header
        if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
        {
            return Results.Problem("Authorization header not found.");
        }

        var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
        var resultado = await httpService.GetList( authorizationHeader.Substring("Bearer ".Length).Trim());
        // return Results.Ok(resultado);
        var myObject = JsonSerializer.Deserialize<TokenResponse>(resultado);
        return Results.Ok(myObject);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("/set-value", (HttpContext context, IRedisService redis) =>
{
    redis.SetStringAsync("Prueba", "Llego");
});

app.MapGet("/get-value", async (IHttpService httpService, HttpContext context, IRedisService redis) =>
{
    // SSOUser objeto = JsonSerializer.Deserialize<SSOUser>(value.ToString());
    
    if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
    {
        return Results.Problem("Authorization header not found.");
    }

    var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
    var token = authorizationHeader.Substring("Bearer ".Length).Trim();
    
    var tokenHandler = new JwtSecurityTokenHandler();
    var jwtToken = tokenHandler.ReadJwtToken(token);

    var name = "sso_cache_:" + jwtToken.Id;
    var value = await redis.GetStringAsync(name);
    if (!value.IsNullOrEmpty)
    {
        string pattern = @"^[a-zA-Z0-9]+:\d+:";

        // Dividir la cadena, pero solo en la primera coincidencia
        string[] partes = Regex.Split(value.ToString(), pattern);
        // string jsonString = "@" + partes[1];

        string nuevo = Helper.EscapeJsonString(partes[1].Replace(";",""));
        // using JsonDocument doc = JsonDocument.Parse(nuevo);

        // var objeto = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
        
        SSOUser objeto = JsonSerializer.Deserialize<SSOUser>(nuevo);
        
        
        return Results.Ok(objeto); // Aquí conviertes el value a string
    }
    
    return Results.NotFound();
});

app.MapGet("/weatherforecast", [Permiso("cualquier cosa")](HttpContext context) =>
    {
        var claimsPrincipal = context.User;
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    /*.WithOpenApi()*/;

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}