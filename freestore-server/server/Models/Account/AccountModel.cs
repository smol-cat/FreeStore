using System.Text.Json.Serialization;
using server.Database;

namespace server.Models.Account;

public class AccountModel : IDeserializable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Last_name { get; set; }
    public int? Level { get; set; }
    public DateTime? Date_created { get; set; }
    public string Role { get; set; }
    public virtual void Deserialize(MySqlCustomReader reader)
    {
        Id = (int)reader["id"];
        Name = (string)reader["name"];
        Last_name = (string)reader["last_name"];
        Date_created = (DateTime?)reader["date_created"];
        Level = (int)reader["level"];
        Role = ((UserRole)Level).ToString();
    }
}

public class PersonalAccountModel : AccountModel
{
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }

    public bool Is_blocked { get; set; }
    [JsonIgnore]
    public string Password { get; set; }

    public override void Deserialize(MySqlCustomReader reader)
    {
        base.Deserialize(reader);
        Email = (string)reader["email"];
        PhoneNumber = (string)reader["phone_number"];
        Password = (string)reader["password"];
        Is_blocked = (bool)reader["is_blocked"];
    }
}

public enum UserRole
{
    Vartotojas = 0,
    Administratorius = 1
}