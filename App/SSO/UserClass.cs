using System.Text.Json.Serialization;

namespace DotNet.Docker.SSO;

public class UserClass
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    
    [JsonPropertyName("employeeCode")]
    public string EmployeeCode { get; set; }
    
    [JsonPropertyName("temporaryEmployeeCode")]
    public string TemporaryEmployeeCode { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }

}