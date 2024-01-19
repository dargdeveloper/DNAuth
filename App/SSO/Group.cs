using System.Text.Json.Serialization;

namespace DotNet.Docker.SSO;

public class Group
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("code")]
    public string Code { get; set; }

}