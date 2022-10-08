
using System.Text.Json.Serialization;

namespace server.Models.Item;

public class ImageModel
{
    public int ChallegeId { get; set; }
    public string ImageName { get; set; }
    public IFormFile Image { get; set; }
}

public class IdImageModel : ImageModel
{
    public int Id { get; set; }
}