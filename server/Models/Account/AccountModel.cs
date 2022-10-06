using System.Text.Json.Serialization;

namespace server.Models.Account;

public class AccountModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    public string Name { get; set; }
    public string LastName { get; set;}
    public string PhoneNumber { get; set; }
    public int Level { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime? Date_created { get; set; }

}