
using Microsoft.AspNetCore.Mvc;
using server.Controllers.ObjectResults;
using server.Entities;

namespace server.Controllers;
public class MainController : ControllerBase
{
    public DbConnection Db = DbConnection.Instance;

    public override OkObjectResult Ok(object? value)
    {
        return base.Ok(value);
    }

    [NonAction]
    public NotAcceptableObjectResult NotAcceptable(object message = null) => new NotAcceptableObjectResult(message);

    [NonAction]
    public ServerErrorObjectResult ServerError(string optionalMessage = null) =>
        new ServerErrorObjectResult(new ServerErrorModel(optionalMessage));

    public ServerErrorObjectResult DatabaseError(string optionalMessage = null) =>
        new ServerErrorObjectResult(new ServerErrorModel(DbConnection.Instance.LastException.Message, optionalMessage, DbConnection.Instance.LastException.StackTrace));
}