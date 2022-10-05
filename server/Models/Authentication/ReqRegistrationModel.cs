
using System.Text.Json.Serialization;

namespace server.Models.Authentication;

public class ReqRegistrationModel
{
    [JsonInclude]
    public string Name { get; set; }
    [JsonInclude]
    public string LastName { get; set; }
    [JsonInclude]
    public string Email { get; set; }
    [JsonInclude]
    public string Password { get; set; }
}