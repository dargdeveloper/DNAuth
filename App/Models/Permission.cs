using System.Text.Json.Serialization;

namespace DotNet.Docker.Models;

public class Permission
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; }
    [JsonPropertyName("role_id")]
    public string Role_id { get; set; }
    [JsonPropertyName("group_id")]
    public string Group_id { get; set; }
    
    // [JsonPropertyName("")]
    // public string FunctionCodes;

}