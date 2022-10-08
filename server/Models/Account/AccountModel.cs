using server.Database;

namespace server.Models.Account;

public class AccountModel : IDeserializable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Last_name { get; set; }
    public DateTime? Date_created { get; set; }
    public virtual void Deserialize(MySqlCustomReader reader)
    {
        Id = (int)reader["id"];
        Name = (string)reader["name"];
        Last_name = (string)reader["last_name"];
        Date_created = (DateTime?)reader["date_created"];
    }
}

public class PersonalAccountModel : AccountModel
{
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int Level { get; set; }
    public bool Is_blocked { get; set; }

    public override void Deserialize(MySqlCustomReader reader)
    {
        base.Deserialize(reader);
        Email = (string)reader["email"];
        try
        {
            PhoneNumber = (string?)reader["phone_number"];
        }
        catch (Exception) { }
        Level = (int)reader["level"];
        Is_blocked = (bool)reader["is_blocked"];
    }
}