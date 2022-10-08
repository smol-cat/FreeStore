

using Microsoft.AspNetCore.Mvc;
using server.Models.Item;

namespace server.Controllers;

[ApiController]
[Route("/api/v1/category/{catID}/item/{itId}/image")]
public class ImageConctroller : MainController
{
    [HttpPost]
    public IActionResult Post(int catId, int itId, IFormFile image)
    {
        if (!ItemController.TryGetItem(catId, itId, out var item))
        {
            return DatabaseError("Failed to find item");
        }

        if (item == null)
        {
            return NotFound(new ResponseModel("Item or item category was not found"));
        }

        MemoryStream stream = new();
        image.CopyTo(stream);
        byte[] imageBytes = stream.ToArray();

        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            ["blob"] = imageBytes,
            ["fk_item_id"] = itId,
            ["image_name"] = image.FileName
        };

        if (!Db.Execute("INSERT INTO image (`blob`, fk_item_id, image_name) VALUES (@blob, @fk_item_id, @image_name)", parameters))
        {
            return DatabaseError("Could not upload image");
        }

        return Created($"/api/v1/category/{catId}/item/{itId}/image/{Db.LastInsertedId}", new ResponseModel("Image has been uploaded"));
    }
}