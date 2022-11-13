
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using server.Database;

namespace server.Utilities;

public class TokenManagerMiddleware
{
    private static DbConnection conn;
    // private TokenCache _cache;
    private readonly RequestDelegate _next;
    private bool initialized;

    public TokenManagerMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task Invoke(HttpContext context, DbConnection connection, TokenCache cache)
    {
        if (conn == null)
        {
            conn = connection;
            Console.WriteLine("DB initialized");
        }
        
        var endpoint = context.GetEndpoint();

        if (!endpoint?.Metadata.Any(e => e is AuthorizeAttribute) ?? true)
        {
            await _next(context);
            return;
        }

        string token = context.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");

        if (token == null)
        {
            await _next(context);
            return;
        }

        if (cache.IsBlocked(token))
        {
            context.Response.Headers.Add("WWW-Authenticate", @"Bearer error=""invalid_token""");
            context.Response.StatusCode = 401;
            return;
        }

        await _next(context);
    }
}