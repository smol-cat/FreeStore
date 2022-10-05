
using Microsoft.AspNetCore.Mvc;
using server.Controllers.ObjectResults;
using server.Entities;

namespace server.Controllers;
public class MainController : ControllerBase
{
    [NonAction]
    public NotAcceptableObjectResult NotAcceptable(object message = null) => new NotAcceptableObjectResult(message);

    [NonAction]
    public ServerErrorObjectResult ServerError(ServerErrorType errorType = ServerErrorType.Regular, string optionalMessage = null)
    {
        switch (errorType)
        {
            case ServerErrorType.Database:
                return new ServerErrorObjectResult(new ServerErrorModel(DbConnection.Instance.LastException.Message, optionalMessage, DbConnection.Instance.LastException.StackTrace));
            default:
                return new ServerErrorObjectResult(new ServerErrorModel(optionalMessage));
        }
    }
}

public enum ServerErrorType
{
    Regular,
    Database
}