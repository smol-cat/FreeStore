

using Microsoft.AspNetCore.Mvc;
using server.Models.Category;

namespace server.Controllers;

[ApiController]
[Route("api/v1/category")]
public class CategoryController : MainController
{

    public static bool TryGetCategory(int id, out IdCategoryModel category)
    {
        category = null;
        if (!Db.SelectAndDeserialize<IdCategoryModel>($"SELECT * FROM category WHERE id = {id} AND unlisted = 0", out var categories))
        {
            return false;
        }

        category = categories.FirstOrDefault();
        return true;
    }

    [HttpGet]
    public IActionResult List()
    {
        if (!Db.SelectAndDeserialize<IdCategoryModel>("SELECT * FROM category WHERE unlisted = 0", out var categories))
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
            return NotAcceptable(new ResponseModel("Fields: description and name are required"));
        }

        if (!Db.Execute($"INSERT INTO category (name, description) VALUES (\"{categoryModel.Name}\", \"{categoryModel.Description}\")"))
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
            return NotAcceptable(new ResponseModel("Fields: description and name are required"));
        }

        if (!TryGetCategory(id, out IdCategoryModel idCategoryModel))
        {
            return DatabaseError("Error finding category");
        }

        if (idCategoryModel == null)
        {
            return NotFound(new ResponseModel("Category is not found"));
        }

        if (!Db.Execute($"UPDATE category SET name = \"{categoryModel.Name}\", description = \"{categoryModel.Description}\" WHERE id = {id}"))
        {
            return DatabaseError("Error occured updating category");
        }

        return Ok(new ResponseModel("Category information has been updated"));
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(int id)
    {
        if (!TryGetCategory(id, out IdCategoryModel idCategoryModel))
        {
            return DatabaseError("Error finding category");
        }

        if (idCategoryModel == null)
        {
            return NotFound(new ResponseModel("Category is not found"));
        }

        if (!Db.Execute($"UPDATE category SET unlisted = 1 WHERE id = {id}"))
        {
            return DatabaseError("Error occured updating category");
        }

        return Ok(new ResponseModel("Category has been removed"));
    }
    
}