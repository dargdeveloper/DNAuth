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

using System.Text;
using System.Text.Json;
using DotNet.Docker;
using DotNet.Docker.CustomAuth;
using DotNet.Docker.Redis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddHttpClient("FormClient", client =>
// {
//     client.BaseAddress = new Uri("http://tuapi.com/");
//     client.DefaultRequestHeaders.Add("Accept", "application/json");
// });

// builder.Services.AddAuthentication()
//     .AddOAuth("sso", o =>
//     {
//         o.ClientId = "9b0e5e01-1474-445e-8496-627d08c61656";
//         o.ClientSecret = "9Tq3EHiuIakfAClpU3Wgm6X0EvVTlzh0ZXJNIOPS";
//     });


// builder.Services.Add

// En tu configuración de servicios
var publicKey = File.ReadAllText("./clave.pem");
var rsaSecurityKey = Helper.GetPublicKey(publicKey);

// Configura la autenticación OAuth 2.0
// builder.Services.AddAuthentication(options =>
//     {
//         // options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(options =>
//     {
//         options.Authority = "https://dev-sso.proyectos-enee.xyz"; // URL de tu servidor OAuth Laravel
//         options.Audience = "9b19c38f-9420-474c-b4f9-f223a1e1c4cf";
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = rsaSecurityKey /*new SymmetricSecurityKey(Encoding.UTF8.GetBytes("-----BEGIN PUBLIC KEY-----\nMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAn7EhGfTC7050eIPV9tIw\nSLqKVp47PosSfN1fA+W7bUUXHu4tgwawIOG0OTiN6NF9G8Ul/0Ah2BPMLiz6hWQO\nz2g4yIfKmRhrF2X/mCkYHFvgA45czAT5dy3WzBUXaLiC/RAbXkYVkUbNOCh68Qg1\neIcNWxqxTQ2/ckZah6+r3D96wLGtXAnFHdLDGntWNJgTz3jygtiquBi/tI+imeDx\n1nBDkbzUwZXFyDnysbAgMh1AlQ92vxaDXFCPhzTT9R56dBKsuqshC0/H4mzed4Fh\nCKbt7xpIJIM/D9a+h27GhyikVNAHR/CQjT4k3Jd7ceFXfNPiYZ3gBYmgrIskiS+x\nqCZfJattTUbhauKLe/eNqH+BMxGVJ/wG63dC1al2NuVWVNG08mFr4dqPwf3jPw66\nZEN5LbQ30eU1HkjDnRlPUAXxcqcfG/++jrEs2uz/0WgyZHSR90pIprN0bOD/ucl4\nSFGmoQ9Kjhrf7tslUUCfJlE4XsDhEGVnCvjQlzIqlOJqqsI/8hywSJK62GqHm1vI\nfmo17Kcg9RUyQQAeKLZLc5/Cwt9ZatXJBg7TnxAB1x4qzSRAXKscZgZ/AOl+mN5u\nuThnDh/S3rnIKO+6+hAwPkWvaGPSbP6kzT/60iaYyqx/4TBwVd3wpqxbc/9Q3O45\n8mP/KH0lIT3K45n1HwlGx0UCAwEAAQ==\n-----END PUBLIC KEY-----"))*/,
//             
//             
//             
//             ValidAudience = options.Audience,
//             // ValidIssuer = options.Authority,
//             
//         };
//         options.RequireHttpsMetadata = false;
//         options.Events = new JwtBearerEvents()
//         {
//             OnAuthenticationFailed = async (c) =>
//             {
//                 
//                 var x = c.Exception.Message;
//                 var headerDictionary = c.HttpContext.Request.Headers;
//             }
//         };
//     });

//Add All Depended Services Here 
builder.Services.AddTransient<CustomJwtBearerHandler>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<JwtBearerOptions, CustomJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, options => { });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    // option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    // {
    //     In = ParameterLocation.Header,
    //     Name = "Authorization",
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