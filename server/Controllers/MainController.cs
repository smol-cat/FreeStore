
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using server.Controllers.ObjectResults;
using server.Database;

namespace server.Controllers;
public class MainController : ControllerBase
{
    protected static DbConnection Db;
    protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    protected string UserRole => User.FindFirstValue(ClaimTypes.Role);

    public MainController(DbConnection db)
    {
        Db = db;
    }

    [NonAction]
    public override OkObjectResult Ok(object? value)
    {
        return base.Ok(value);
    }

    // [NonAction]
    // public NotAcceptableObjectResult NotAcceptable(object message = null) => new NotAcceptableObjectResult(message);

    [NonAction]
    public ServerErrorObjectResult ServerError(string optionalMessage = null) =>
        new ServerErrorObjectResult(new ServerErrorModel(optionalMessage));

    [NonAction]
    public ServerErrorObjectResult DatabaseError(string optionalMessage = null) =>
        new ServerErrorObjectResult(new ServerErrorModel(Db.LastException.Message, optionalMessage, Db.LastException.StackTrace));
}