

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using tests;
using tests.utils;

public class DatabaseStressTests
{
    RestClient client = new(tests.Globals.ApiRoot);
    private CredentialsModel credentials = new CredentialsModel(
        "", "", "ernunt@gmail.com", "123456789");
    string authorizationToken;

    public List<string> endpoints = new(){
        "/accounts/1",
        "/accounts/1",
        "/accounts/1",
        "/accounts/1",
        "/accounts/1",
        "/categories",
        "/categories/1/items",
        "/categories/1/items",
        "/categories/1/items",
        "/categories/1/items",
        "/categories",
        "/categories",
        "/categories",
        "/categories",
        "/categories",
        "/categories",
        "/categories",
        "/categories",
        "/categories",
        "/accounts/own",
        "/accounts/own",
        "/accounts/own",
        "/accounts/own",
        "/accounts/1",
        "/accounts/1",
        "/accounts/1",
        "/accounts/1",
        "/accounts/1",
    };

    [Test]
    public void MultithreadedRequests_AllSuccess()
    {
        RestRequest request = new("/tokens", Method.Post);
        request.AddJsonBody(credentials);
        if (!TryPerformRequest(request, out var response))
        {
            Assert.Fail("Request failed " + response.StatusCode + " " + response.Content);
        }

        authorizationToken = (string)JObject.Parse(response.Content)["token"];

        var responses = new ConcurrentBag<RestResponse>();

        Parallel.ForEach(endpoints, e =>
        {
            var localClient = new RestClient(tests.Globals.ApiRoot);
            request = new(e);
            TryPerformRequest(request, out var response, localClient);
            responses.Add(response);
        });

        responses.ToList().ForEach(e => {
            Console.WriteLine(e.StatusCode);
        });
    }

    private bool TryPerformRequest(RestRequest request, out RestResponse response, RestClient? customClient = null)
    {
        customClient ??= client;
        request.AddHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(authorizationToken))
        {
            request.AddHeader("Authorization", $"Bearer {authorizationToken}");
        }

        response = customClient.Execute(request);
        return response.StatusCode is HttpStatusCode.OK or HttpStatusCode.NoContent or HttpStatusCode.Created;
    }

}