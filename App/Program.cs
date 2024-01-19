using System.Text.Json;
using DotNet.Docker;
using DotNet.Docker.Auth;
using DotNet.Docker.Helpers;
using DotNet.Docker.Redis;
using DotNet.Docker.SSO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen();

// Registro de servicios
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IHelper, Helper>();

// Agrega esta línea para registrar HttpClient
builder.Services.AddHttpClient();
// Add services to the container.
builder.Logging.AddConsole();

builder.Services.AddHttpContextAccessor();

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
        var resultado = await httpService.EnviarFormularioAsync(data);
        
        var myObject = JsonSerializer.Deserialize<TokenResponse>(resultado);
        return Results.Ok(myObject);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/get-value", async (IRedisService redis, IHelper helper) =>
{
    var jwtToken = helper.ReadJwtToken();

    var name = "sso_cache_:" + jwtToken.Id;
    var value = await redis.GetStringAsync(name);
    if (!value.IsNullOrEmpty)
    {
        SSOUser objeto = redis.GetUser(value.ToString());
        
        return Results.Ok(objeto); // Aquí conviertes el value a string
    }
    
    return Results.NotFound();
});

app.Run();