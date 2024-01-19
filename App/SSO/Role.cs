using System.Text.Json.Serialization;

namespace DotNet.Docker.SSO;

public class Role
{
    [JsonPropertyName("roleId")]
    public string RoleId { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("projectId")]
    public string ProjectId { get; set; }

}