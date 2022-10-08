

using System.Runtime.Serialization;

namespace server.Models.Item;

public class ItemModel
{
    public decimal? Price { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public int? CategoryId { get; set;}
}