
using System.Text.Json.Serialization;
using MySql.Data.MySqlClient;

namespace server.Models.Category;
public class CategoryModel
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class IdCategoryModel : CategoryModel, IDeserializable
{
    public int Id { get; set; }

    public void Deserialize(MySqlDataReader reader)
    {
        Id = (int)reader["id"];
        Name = (string)reader["name"];
        Description = (string)reader["description"];
    }
}