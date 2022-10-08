

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

        Dictionary<string, object> parameters = new()
        {
            ["id"] = id,
            ["category_id"] = catId
        };

        if (!Db.SelectAndDeserialize<DetailedItemModel>("SELECT * FROM detailed_item WHERE id = @id AND category_id = @category_id AND unlisted = 0", parameters, out var items))
        {
            return false;
        }

        itemModel = items.FirstOrDefault();
        return true;
    }

    [HttpGet]
    [Route("{itId}")]
    public IActionResult Get(int catId, int itId)
    {
        if (!TryGetItem(catId, itId, out var itemModel))
        {
            return DatabaseError("Failed to get an item");
        }

        if (itemModel == null)
        {
            return NotFound(new ResponseModel("Item or category was not found"));
        }

        return Ok(itemModel);
    }

    [HttpGet]
    public IActionResult GetList(int catId)
    {
        if (!CategoryController.TryGetCategory(catId, out IdCategoryModel categoryModel))
        {
            return DatabaseError("Could not find category");
        }

        if (categoryModel == null)
        {
            return NotFound(new ResponseModel("Category was not found"));
        }

        if (!Db.SelectAndDeserialize<DetailedItemModel>("SELECT * FROM detailed_item WHERE category_id = @category_id AND unlisted = 0", new() { ["category_id"] = catId }, out List<DetailedItemModel> items))
        {
            return DatabaseError("Could not get list of items");
        }

        return Ok(items);
    }

    [HttpPost]
    public IActionResult PostItem(int catId, ItemModel itemModel)
    {
        if (itemModel.Price == null)
        {
            return NotAcceptable(new ResponseModel("Price is not provided"));
        }

        if (string.IsNullOrEmpty(itemModel.Title))
        {
            return NotAcceptable(new ResponseModel("Title should be included"));
        }

        if (!CategoryController.TryGetCategory(catId, out IdCategoryModel categoryModel))
        {
            return DatabaseError("Could not find category");
        }

        if (categoryModel == null)
        {
            return NotFound(new ResponseModel("Category was not found"));
        }

        int accountId = 1;
        Dictionary<string, object> parameters = new()
        {
            ["title"] = itemModel.Title,
            ["description"] = itemModel.Description,
            ["price"] = itemModel.Price,
            ["fk_category_id"] = catId,
            ["fk_account_id"] = accountId
        };

        if (!Db.Execute("INSERT INTO item (title, description, price, fk_category_id, fk_account_id) VALUES (@title, @description, @price, @fk_category_id, @fk_account_id)", parameters))
        {
            return DatabaseError("Could not insert item");
        }

        return Created($"/api/v1/category/{catId}/item/{Db.LastInsertedId}", new ResponseModel("Item has been posted"));
    }

    [HttpPut]
    [Route("itId")]
    public IActionResult EditItem(int catId, int itId, ItemModel itemModel)
    {
        if (itemModel.Price == null)
        {
            return NotAcceptable(new ResponseModel("Price is not provided"));
        }

        if (string.IsNullOrEmpty(itemModel.Title))
        {
            return NotAcceptable(new ResponseModel("Title should be included"));
        }

        if (!TryGetItem(catId, itId, out var _))
        {
            return NotFound("Item was not found");
        }

        Dictionary<string, object> parameters = new()
        {
            ["id"] = itId,
            ["title"] = itemModel.Title,
            ["description"] = itemModel.Description,
            ["price"] = itemModel.Price,
            ["fk_category_id"] = itemModel.CategoryId ?? catId,
        };

        if (!Db.Execute("UPDATE item SET title = @title, description = @description, fk_category_id = @fk_category_id WHERE id = @id", parameters))
        {
            return DatabaseError("Error occured trying to update item");
        }

        return Ok(new ResponseModel("Item information has been updated"));
    }

    [HttpPatch]
    [Route("{itId}/changeStatus")]
    public IActionResult ChangeStatus(int catId, int itId, StatusModel statusModel)
    {
        if (statusModel.Status_id == null)
        {
            return NotAcceptable(new ResponseModel("Status id was not provided"));
        }

        if (!Db.SelectAndDeserialize("SELECT * FROM state WHERE id = @id", new() { ["id"] = statusModel.Status_id }, out var statusList))
        {
            return DatabaseError("Failed to fetch list of states");
        }

        if (!statusList.Any())
        {
            return NotFound(new ResponseModel("Provided item state was not found"));
        }

        if (!TryGetItem(catId, itId, out var _))
        {
            return NotFound("Item or item category was not found");
        }

        Dictionary<string, object> parameters = new()
        {
            ["id"] = itId,
            ["state"] = statusModel.Status_id
        };

        if (!Db.Execute("UPDATE item SET fk_state_id = @state WHERE id = @id", parameters))
        {
            return DatabaseError("Error occured updating item status");
        }

        return Ok(new ResponseModel("Item status was updated"));
    }

    [HttpDelete]
    [Route("{itId}")]
    public IActionResult RemoveItem(int catId, int itId)
    {
        if (!TryGetItem(catId, itId, out var itemModel))
        {
            return DatabaseError("Failed to get an item");
        }

        if (itemModel == null)
        {
            return NotFound(new ResponseModel("Item or category was not found"));
        }

        if (!Db.Execute("UPDATE item SET unlisted = 1 WHERE id = @id", new() { ["id"] = itId }))
        {
            return DatabaseError("Error occured while deleting item");
        }

        return Ok(new ResponseModel("Item has been successfully removed from the store"));
    }
}