

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.OpenApi.Extensions;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using server.Models.Item;
using tests;
using tests.utils;

public class DatabaseStressTests
{
    // RestClient client = new(tests.Globals.ApiRoot);
    private CredentialsModel credentials = new CredentialsModel(
        "", "", "ernunt@gmail.com", "123456789");
    
    private int userCount = 1000;
    private int testDurationSeconds = 300;
    private bool finish = false;
    
    private RestUserData[] data;
    
    private List<RequestData> randomRequests = new()
    {
        new("/categories/1/items/651/comments", Method.Get, null),
        new("/categories/1/items/651/comments", Method.Post, new CommentModel(){ Text = "Test comment"}),
        new("/accounts/4", Method.Get, null),
        new("/accounts/own", Method.Get, null),
        new("/categories", Method.Get, null),
        new("/categories/1/items/651", Method.Get, null),
        new("/categories/1/items", Method.Post, new ItemModel() { Price = 0, Description = "Test Description", Title = "Test Title"}),
    };

    private CredentialsModel GetGeneratedUser(int i) => new CredentialsModel($"UserName_{i}", $"UserLastName_{i}", $"user{i}@gmail.com", $"UserPassword_{i}");

    private bool Login(int i, RestClient client, out string authorization)
    {
        authorization = null;
        RestRequest loginRequest = new RestRequest("/tokens", Method.Post);
        loginRequest.AddJsonBody(GetGeneratedUser(i));
        if (!TryPerformRequest(loginRequest, out var response, i, client))
        {
            RestRequest registerRequest = new RestRequest("/accounts", Method.Post);
            registerRequest.AddJsonBody(GetGeneratedUser(i));
            if (!TryPerformRequest(registerRequest, out response, i, client))
            {
                return false;
            }

            if (!TryPerformRequest(loginRequest, out response, i, client))
            {
                return false;
            }
            
        }
        
        authorization = (string)JObject.Parse(response.Content)["token"];
        return true;
    }

    private void SimulateUser(object i)
    {
        int index = (int)i;
        RestClient client = new RestClient(tests.Globals.ApiRoot);
        data[index] = new();
        Random random = new();
        
        if (!Login(index, client, out string authorization))
        {
            return;
        }

        while (!finish)
        {
            RequestData randomRequest = randomRequests[random.Next(randomRequests.Count)];
            RestRequest request = new(randomRequest.Resource, randomRequest.Method);
            if (randomRequest.Body != null)
            {
                request.AddJsonBody(randomRequest.Body);
            }

            TryPerformRequest(request, out _, index, client, authorization);
        }
    }

    [Test]
    public void MultithreadedRequests_AllSuccess()
    {
        Thread[] threads = new Thread[userCount];
        data = new RestUserData[userCount];

        for (int i = 0; i < userCount; i++)
        {
            threads[i] = new Thread(SimulateUser);
            threads[i].Start(i);
        }
        
        Thread.Sleep(testDurationSeconds * 1000);
        finish = true;
        
        for (int i = 0; i < userCount; i++)
        {
            threads[i].Join();
        }

        ListAggregatedData();
        
        Assert.Pass();
    }

    private void ListAggregatedData()
    {
        RestUserData aggregatedData = new();
        HashSet<HttpStatusCode> codes = new();

        foreach (RestUserData entry in data)
        {
            foreach (var (key, responses) in entry.Requests)
            {
                aggregatedData.Requests.TryAdd(key, new List<ResponseData>());
                aggregatedData.Requests[key].AddRange(responses);
                responses.GroupBy(e => e.StatusCode).Select(e => e.Key).ToList().ForEach(e => codes.Add(e));
            }
        }

        List<HttpStatusCode> responseCodes = codes.ToList().OrderBy(e => (int)e).ToList();
        string header = String.Format($"|{"resource",-40}|{"method",-20}|{"performed count",-40}|{"average response time (s)",-40}|");
        responseCodes.ForEach(e => header += String.Format($"{e, -20}|"));
        
        Console.WriteLine(header);
        foreach (var (key, responses) in aggregatedData.Requests)
        {
            int responsesTotal = responses.Count;
            float averageResponseTime = responses.Average(e => e.Duration);
            string entry = $"|{key.Resource,-40}|{key.Method,-20}|{responsesTotal,-40}|{averageResponseTime,-40}|";
            foreach (HttpStatusCode code in responseCodes)
            {
                float percentage = ((float)responses.Count(e => e.StatusCode == code) / responsesTotal) * 100;
                entry += Strings.Format($"{$"{percentage}%",-20}|");
            }
            Console.WriteLine(entry);
        }
    }

    private bool TryPerformRequest(RestRequest request, out RestResponse response, int i, RestClient customClient, string authorizationToken = null)
    {
        // customClient ??= client;
        request.AddHeader("Content-Type", "application/json");

        if (!string.IsNullOrEmpty(authorizationToken))
        {
            request.AddHeader("Authorization", $"Bearer {authorizationToken}");
        }

        Stopwatch sw = new();
        sw.Start();
        response = customClient.Execute(request);
        sw.Stop();
        float responseTime = (float)((double)sw.ElapsedMilliseconds / 1000);
        RequestLogData reqLogData = new(request.Resource, request.Method);
        
        data[i].Requests.TryAdd(reqLogData, new());
        data[i].Requests[reqLogData].Add(new ResponseData(response.StatusCode, responseTime));
        
        return response.StatusCode is HttpStatusCode.OK or HttpStatusCode.NoContent or HttpStatusCode.Created;
    }

}