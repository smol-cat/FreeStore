

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Database;
using server.Models.Authentication;

namespace server.Controllers;

[ApiController]
[Route("/api/v1/categories/{catId}/items/{itId}/comments")]
[Authorize(Roles = Role.Everyone)]
public class CommentController : MainController
{
    public CommentController(DbConnection db) : base(db)
    {
    }

    public static bool TryFindComment(int catId, int itId, int commId, out DetailedCommentModel model)
    {
        model = null;
        if (!ItemController.TryGetItem(catId, itId, out var itemModel))
        {
            return false;
        }

        if (itemModel == null)
        {
            return true;
        }

        if (!Db.SelectAndDeserialize<DetailedCommentModel>("SELECT * FROM detailed_comment WHERE id = @commId AND item_id = @itId", new() { ["commId"] = commId, ["itId"] = itId }, out var comments))
        {
            return false;
        }

        model = comments.FirstOrDefault();
        return true;
    }

    [HttpGet]
    public IActionResult GetList(int catId, int itId)
    {
        if (!ItemController.TryGetItem(catId, itId, out var itemModel))
        {
            return DatabaseError("Failed to get an item");
        }

        if (itemModel == null)
        {
            return NotFound(new ResponseModel("Item or category was not found"));
        }

        if (!Db.SelectAndDeserialize<DetailedCommentModel>("SELECT * FROM detailed_comment WHERE item_id = @item_id", new() { ["item_id"] = itId }, out var comments))
        {
            return DatabaseError("Could not get a list of comments");
        }

        return Ok(comments);
    }

    [HttpGet]
    [Route("{commId}")]
    public IActionResult Get(int catId, int itId, int commId)
    {
        if (!TryFindComment(catId, itId, commId, out var model))
        {
            return DatabaseError("Failed to get an item");
        }

        if (model == null)
        {
            return NotFound(new ResponseModel("Comment was not found"));
        }

        return Ok(model);
    }

    [HttpPost]
    public IActionResult Post(int catId, int itId, CommentModel model)
    {
        if (string.IsNullOrEmpty(model.Text))
        {
            return BadRequest(new ResponseModel("Comment should not be empty"));
        }

        if (!ItemController.TryGetItem(catId, itId, out var itemModel))
        {
            return DatabaseError("Failed to get an item");
        }

        if (itemModel == null)
        {
            return NotFound(new ResponseModel("Item or category was not found"));
        }

        Dictionary<string, object> parameters = new()
        {
            ["text"] = model.Text,
            ["fk_account_id"] = UserId,
            ["fk_item_id"] = itId,
            ["date_created"] = DateTime.Now.ToString(Globals.MySqlDateFormat),
        };

        if (!Db.Execute("INSERT INTO comment (text, fk_account_id, fk_item_id, date_created) VALUES (@text, @fk_account_id, @fk_item_id, @date_created)", parameters))
        {
            return DatabaseError("Error occured inserting the comment");
        }

        return Created($"/api/v1/category/{catId}/item/{itId}/comment/{Db.LastInsertedId}", new ResponseModel("Comment has been posted"));
    }

    [HttpPut]
    [Route("{commId}")]
    public IActionResult Edit(int catId, int itId, int commId, CommentModel model)
    {
        if (string.IsNullOrEmpty(model.Text))
        {
            return BadRequest(new ResponseModel("Comment should not be empty"));
        }

        if (!TryFindComment(catId, itId, commId, out var commentModel))
        {
            return DatabaseError("Error occured trying to find a comment");
        }

        if (commentModel == null)
        {
            return NotFound(new ResponseModel("Comment was not found"));
        }

        if (commentModel.Account.Id.ToString() != UserId)
        {
            return Forbid();
        }

        if (!Db.Execute("UPDATE comment SET text = @text WHERE id = @id", new() { ["text"] = model.Text, ["id"] = commId }))
        {
            return DatabaseError("Failed to update the comment");
        }

        return Ok(new ResponseModel("Comment has been updated"));
    }

    [HttpDelete]
    [Route("{commId}")]
    public IActionResult Delete(int catId, int itId, int commId)
    {
        if (!TryFindComment(catId, itId, commId, out var commentModel))
        {
            return DatabaseError("Error occured trying to find a comment");
        }

        if (commentModel == null)
        {
            return NotFound(new ResponseModel("Comment was not found"));
        }

        if (commentModel.Account.Id.ToString() != UserId && !User.IsInRole(Role.Admin))
        {
            return Forbid();
        }

        if (!Db.Execute("DELETE FROM comment WHERE id = @id", new() { ["id"] = commId }))
        {
            return DatabaseError("Failed to delete the comment");
        }

        return NoContent();
    }
}