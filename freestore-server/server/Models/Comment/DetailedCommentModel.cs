

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
        Text = (string)reader["text"];
        Date_created = (DateTime)reader["date_created"];

        if (reader["name"] != null)
        {
            Account = new AccountModel();
            Account.Id = (int)reader["account_id"];
            Account.Name = (string)reader["name"];
            Account.Last_name = (string)reader["last_name"];
            Account.Level = (int)reader["account_level"];
        }
    }
}