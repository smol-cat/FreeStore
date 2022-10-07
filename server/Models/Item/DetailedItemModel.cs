
using MySql.Data.MySqlClient;
using server.Models.Account;
using server.Models.Category;
using server.Models.Other;

namespace server.Models.Item;
public class DetailedItemModel : IDeserializable
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<int> Image_ids { get; set; }
    public CategoryModel Category { get; set; }
    public AccountModel Account { get; set; }
    public StateModel State { get; set; }

    public void Deserialize(MySqlDataReader reader)
    {
        Id = (int)reader["id"];
        Title = (string)reader["title"];
        Description = (string)reader["description"];
        Image_ids = ((string)reader["image_ids"]).Split(";").Select(e => int.Parse(e)).ToList();
        Category = new IdCategoryModel
        {
            Id = (int)reader["category_id"],
            Name = (string)reader["category_name"],
        };

        Account = new AccountModel
        {
            Id = (int)reader["account_id"],
            Name = (string)reader["account_name"],
            Last_name = (string)reader["account_last_name"],
        };

        State = new StateModel
        {
            Id = (int)reader["state_id"],
            Name = (string)reader["state_name"],
        };

    }
}