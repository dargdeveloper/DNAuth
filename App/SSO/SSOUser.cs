using System.Text.Json.Serialization;

namespace DotNet.Docker.SSO;

using System;
using System.Collections.Generic;

public class SSOUser
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; }
    
    [JsonPropertyName("user")]
    public UserClass User { get; set; }
    
    [JsonPropertyName("groups")]
    public ICollection<Group> Groups { get; set; }
    [JsonPropertyName("roles")]
    public ICollection<Role> Roles { get; set; }
    [JsonPropertyName("modules")]
    public ICollection<Module> Modules { get; set; }
    [JsonPropertyName("functions")]
    public ICollection<Function> Functions { get; set; }
    [JsonPropertyName("permissions")]
    public ICollection<Permission> Permissions { get; set; }
    [JsonPropertyName("expireToken")]
    public object ExpireToken { get; set; } // Can be TimeSpan, DateTime, or int
    [JsonPropertyName("expireRefresh")]
    public object ExpireRefresh { get; set; } // Can be TimeSpan, DateTime, or int
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }

    public SSOUser(
        string uuid = null,
        UserClass user = null,
        ICollection<Group> groups = null,
        ICollection<Role> roles = null,
        ICollection<Module> modules = null,
        ICollection<Function> functions = null,
        ICollection<Permission> permissions = null,
        object expireToken = null, // Can be TimeSpan, DateTime, or int
        object expireRefresh = null, // Can be TimeSpan, DateTime, or int
        string token = null,
        string refreshToken = null
    )
    {
        Uuid = uuid;
        User = user;
        Groups = groups ?? new List<Group>();
        Roles = roles ?? new List<Role>();
        Modules = modules ?? new List<Module>();
        Functions = functions ?? new List<Function>();
        Permissions = permissions ?? new List<Permission>();
        ExpireToken = expireToken;
        ExpireRefresh = expireRefresh;
        Token = token;
        RefreshToken = refreshToken;
    }
}

// Note: You will need to define the Group, Role, Module, Function, and Permission classes or use appropriate existing classes.

