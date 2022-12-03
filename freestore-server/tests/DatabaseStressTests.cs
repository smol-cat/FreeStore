

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
    
    private static Random rng = new Random();  

    public List<string> endpoints = new(){
        "/accounts/4",
        "/accounts/4",
        "/accounts/4",
        "/accounts/4",
        "/accounts/4",
        "/categories",
        "/categories/1/items/1/comments",
        "/categories/1/items/1/comments",
        "/categories/1/items/1/comments",
        "/categories/1/items/1/comments",
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
        "/accounts/4",
        "/accounts/4",
        "/accounts/4",
        "/accounts/4",
        "/accounts/4",
    };

    public List<string> postEndpoints = new()
    {
        "/tokens",
        "/tokens",
        "/tokens",
        "/tokens",
        "/tokens",
        "/tokens",
        "/tokens",
        "/tokens",
        "/tokens",
        "/tokens",
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

        List<RestRequest> requests = endpoints.Select(e => new RestRequest(e)).ToList();
        requests.AddRange(postEndpoints.Select(e => new RestRequest(e, Method.Post).AddJsonBody(credentials)).ToList());

        Parallel.ForEach(requests, request =>
        {
            var localClient = new RestClient(tests.Globals.ApiRoot);
            TryPerformRequest(request, out var response, localClient);
            responses.Add(response);
        });

        responses.ToList().ForEach(e => {
            Console.WriteLine(e.StatusCode);
        });
    }
    
    private void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
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