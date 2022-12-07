using System;
using System.Net;
using RestSharp;

namespace tests.utils;

public record CredentialsModel(string Name, string LastName, string Email, string Password)
{
    public long Id;
}

public record ResponseData(HttpStatusCode StatusCode, float Duration);

public record RequestLogData(string Resource, Method Method)
{
    public override int GetHashCode() => HashCode.Combine(Resource, Method);
}

public record RequestData(string Resource, Method Method, object Body); 
