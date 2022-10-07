using System.Text.Json.Serialization;
using MySql.Data.MySqlClient;

namespace server.Models.Account;

public class AccountModel : IDeserializable
{
    public int Id { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    public string Name { get; set; }
    public string Last_name { get; set; }
    public string PhoneNumber { get; set; }
    public int Level { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime? Date_created { get; set; }

    public void Deserialize(MySqlDataReader reader)
    {
        Id = (int)reader["id"];
        Email = (string)reader["email"];
        Password = (string)reader["password"];
        Name = (string)reader["name"];
        Last_name = (string)reader["last_name"];
        PhoneNumber = (string)reader["phone_number"];
        Level = (int)reader["level"];
        IsBlocked = (bool)reader["is_blocked"];
        Date_created = (DateTime?)reader["date_created"];
    }
}