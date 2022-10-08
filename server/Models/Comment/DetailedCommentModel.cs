

using server.Database;
using server.Models;
using server.Models.Account;

public class DetailedCommentModel : IDeserializable
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Date_created { get; set; }
    public AccountModel Account { get; set; }

    public void Deserialize(MySqlCustomReader reader)
    {
        Id = (int)reader["id"];
        Text = (string)reader["Text"];
        Date_created = (DateTime)reader["date_created"];
        Account = new AccountModel
        {
            Id = (int)reader["account_id"],
            Name = (string)reader["name"],
            Last_name = (string)reader["last_name"],
        };
    }
}