
using System.Text.Json.Serialization;

namespace server.Models.Category;
public class CategoryModel
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class IdCategoryModel : CategoryModel
{
    public int Id { get; set; }
}