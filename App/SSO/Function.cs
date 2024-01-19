using System.Text.Json.Serialization;

namespace DotNet.Docker.SSO;

public class Function
{
    [JsonPropertyName("functionId")]
    public string FunctionId { get; set; }
    
    [JsonPropertyName("code")]
    public string Code { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("moduleCode")]
    public string ModuleCode { get; set; }
    
    [JsonPropertyName("projectId")]
    public string ProjectId { get; set; }
}