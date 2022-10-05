

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace server.Controllers.ObjectResults;

[DefaultStatusCode(406)]
public class NotAcceptableObjectResult : ObjectResult
{
    public NotAcceptableObjectResult(object? value = null) : base(value)
    {
        StatusCode = 406;
    }
}

[DefaultStatusCode(500)]
public class ServerErrorObjectResult : ObjectResult
{
    public ServerErrorObjectResult(object? value = null) : base(value)
    {
        StatusCode = 500;
    }
}