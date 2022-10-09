

using Microsoft.AspNetCore.Mvc;
using server.Models.Category;

namespace server.Controllers;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController : MainController
{

    public static bool TryGetCategory(int id, out IdCategoryModel category)
    {
        category = null;
        if (!Db.SelectAndDeserialize<IdCategoryModel>($"SELECT * FROM category WHERE id = @id AND unlisted = 0", new() { ["id"] = id }, out var categories))
        {
            return false;
        }

        category = categories.FirstOrDefault();
        return true;
    }

    [HttpGet]
    public IActionResult List()
    {
        if (!Db.SelectAndDeserialize<IdCategoryModel>("SELECT * FROM category WHERE unlisted = 0", new(), out var categories))
        {
            return DatabaseError("Error fetching list of categories");
        }

        return Ok(categories);
    }

    [HttpPost]
    public IActionResult Add(CategoryModel categoryModel)
    {
        if (string.IsNullOrEmpty(categoryModel.Description) || string.IsNullOrEmpty(categoryModel.Name))
        {
            return BadRequest(new ResponseModel("Fields: description and name are required"));
        }

        Dictionary<string, object> parameters = new()
        {
            ["name"] = categoryModel.Name,
            ["description"] = categoryModel.Description,
        };

        if (!Db.Execute($"INSERT INTO category (name, description) VALUES (@name, @description)", parameters))
        {
            return DatabaseError("Failed to insert into categories");
        }

        return Created($"/api/v1/category/{Db.LastInsertedId}", new ResponseModel("Category is created"));
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update(int id, CategoryModel categoryModel)
    {
        if (string.IsNullOrEmpty(categoryModel.Description) || string.IsNullOrEmpty(categoryModel.Name))
        {
            return BadRequest(new ResponseModel("Fields: description and name are required"));
        }

        if (!TryGetCategory(id, out IdCategoryModel idCategoryModel))
        {
            return DatabaseError("Error finding category");
        }

        if (idCategoryModel == null)
        {
            return NotFound(new ResponseModel("Category is not found"));
        }

        Dictionary<string, object> parameters = new()
        {
            ["name"] = categoryModel.Name,
            ["description"] = categoryModel.Description,
            ["id"] = id
        };

        if (!Db.Execute($"UPDATE category SET name = @name, description = @description WHERE id = @id", parameters))
        {
            return DatabaseError("Error occured updating category");
        }

        return Ok(new ResponseModel("Category information has been updated"));
    }

    [HttpPatch]
    [Route("{id}")]
    public IActionResult Unlist(int id)
    {
        if (!TryGetCategory(id, out IdCategoryModel idCategoryModel))
        {
            return DatabaseError("Error finding category");
        }

        if (idCategoryModel == null)
        {
            return NotFound(new ResponseModel("Category is not found"));
        }

        if (!Db.Execute($"UPDATE category SET unlisted = 1 WHERE id = @id", new() { ["id"] = id }))
        {
            return DatabaseError("Error occured updating category");
        }

        return NoContent();
    }
}