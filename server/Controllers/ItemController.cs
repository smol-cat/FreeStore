

using Microsoft.AspNetCore.Mvc;
using server.Models.Category;
using server.Models.Item;

namespace server.Controllers;

[ApiController]
[Route("/api/v1/category/{catId}/item")]
public class ItemController : MainController
{

    public static bool TryGetItem(int catId, int id, out DetailedItemModel itemModel)
    {
        itemModel = null;

        if (!CategoryController.TryGetCategory(catId, out IdCategoryModel categoryModel))
        {
            return false;
        }

        if (categoryModel == null)
        {
            return true;
        }

        if (!Db.SelectAndDeserialize<DetailedItemModel>("", out var items))
        {
            return false;
        }

        itemModel = items.FirstOrDefault();
        return true;
    }

    [HttpGet]
    public IActionResult Get(int catId)
    {
        if (!CategoryController.TryGetCategory(catId, out IdCategoryModel categoryModel) || categoryModel == null)
        {
            return NotFound("Category is not found");
        }
        return Ok();
    }
}