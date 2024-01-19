using System.Text.Json.Serialization;

namespace DotNet.Docker.SSO;

public class Module
{
    [JsonPropertyName("moduleId")]
    public string ModuleId { get; set; }
    
    [JsonPropertyName("code")]
    public string Code { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("padreId")]
    public string? PadreId { get; set; }
    
    [JsonPropertyName("projectId")]
    public string ProjectId { get; set; }

}