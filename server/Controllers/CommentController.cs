

// using Microsoft.AspNetCore.Mvc;

// namespace server.Controllers;

// [ApiController]
// [Route("/api/v1/category/{catId}/item/{itId}/comment")]
// public class CommentController : MainController
// {

//     [HttpGet]
//     public IActionResult GetList(int catId, int itId)
//     {
//         if (!ItemController.TryGetItem(catId, itId, out var itemModel))
//         {
//             return DatabaseError("Failed to get an item");
//         }

//         if (itemModel == null)
//         {
//             return NotFound(new ResponseModel("Item or category was not found"));
//         }

//         // if(!Db.SelectAndDeserialize)
//     }
// }