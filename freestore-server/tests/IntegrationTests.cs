using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serializers;
using server.Database;
using server.Models.Item;
using tests.utils;

namespace tests;

public class IntegrationTests
{
    private DbConnection dbConnection = TestsSetup.Db;
    private RestClient client = TestsSetup.Client;

    private NameValueCollection defaultHeaders;
    private string? authorizationToken;

    private CredentialsModel credentials = new CredentialsModel(
        "TestName", "TestLastname", "test@test.com", "test123456");

    private ItemModel postModel = new ItemModel()
    {
        Price = 10,
        Title = "TestItem",
        Description = "TestDescription",
    };
    
    private CommentModel commentModel = new CommentModel()
    {
        Text = "TestComment"
    };

    private long postId = -1;
    
    [Test]
    public void _1_Create_An_Account_And_Login()
    {
        RestRequest request = new("/accounts", Method.Post);
        request.AddJsonBody(credentials);
        if (!TryPerformRequest(request, out RestResponse response))
        {
            Assert.Fail("Request failed " + response.StatusCode + " " + response.Content);
        }
        
        JObject jsonBody = JObject.Parse(response.Content);
        Assert.IsTrue(jsonBody.ContainsKey("message"));

        string resourceLocation = (string)response.Headers.First(e => e.Name == "Location").Value;
        Match match = utils.Regexes.CreatedId.Match(resourceLocation);
        Assert.IsTrue(match.Success, "Could not match returned account id");
        
        long accountId = long.Parse(match.Value);
        credentials.Id = accountId;
        TestsSetup.TrackInsertedEntries("account", accountId);
        
        Assert.IsTrue(dbConnection.SelectAndDeserialize("SELECT name, last_name, email FROM account WHERE id = @id",
            new() { ["id"] = (int)accountId }, out var results));
        
        Assert.IsNotEmpty(results, "Account does not exist in the database");
        var account = results.First();
        Assert.AreEqual(account["name"], credentials.Name);
        Assert.AreEqual(account["last_name"], credentials.LastName);
        Assert.AreEqual(account["email"], credentials.Email);
        
        request = new("/tokens", Method.Post);
        request.AddJsonBody(credentials);
        if (!TryPerformRequest(request, out response))
        {
            Assert.Fail("Request failed " + response.StatusCode + " " + response.Content);
        }

        authorizationToken = (string)JObject.Parse(response.Content)["token"];
        Assert.IsNotEmpty(authorizationToken);
    }

    [Test]
    public void _2_Create_A_Post()
    {
        Assert.IsNotNull(authorizationToken, "You need to be logged in");
        Assert.IsNotEmpty(authorizationToken, "You need to be logged in");
        
        RestRequest request = new RestRequest("/categories/1/items", Method.Post);
        request.AddJsonBody(postModel);
        Assert.IsTrue(TryPerformRequest(request, out var response), "Item post failed " + response.StatusCode + " " + response.Content);
        
        JObject jsonBody = JObject.Parse(response.Content);
        Assert.IsTrue(jsonBody.ContainsKey("message"));
        
        string resourceLocation = (string)response.Headers.First(e => e.Name == "Location")?.Value;
        Match match = utils.Regexes.CreatedId.Match(resourceLocation);
        Assert.IsTrue(match.Success, "Could not match returned post id");
        postId = long.Parse(match.Value);
        
        TestsSetup.TrackInsertedEntries("item", postId, 1);
        
        Assert.IsTrue(dbConnection.SelectAndDeserialize("SELECT id, title, description, price, fk_category_id, fk_account_id FROM item WHERE id = @id", 
            new() { ["id"] = (int)postId }, out var results));
        Assert.IsNotEmpty(results, "Created post was not found in database");

        var post = results.First();
        Assert.AreEqual(postModel.Title, post["title"]);
        Assert.AreEqual(postModel.Description, post["description"]);
        Assert.AreEqual(postModel.Price, post["price"]);
        Assert.AreEqual(1, post["fk_category_id"]);
        Assert.AreEqual(credentials.Id, post["fk_account_id"]);
    }

    [Test]
    public void _3_Post_A_Comment()
    {
        Assert.IsNotNull(authorizationToken, "You need to be logged in");
        Assert.IsNotEmpty(authorizationToken, "You need to be logged in");
        Assert.AreNotEqual(postId, -1, "Post should be created");
        
        RestRequest request = new RestRequest($"/categories/1/items/{postId}/comments", Method.Post);
        request.AddJsonBody(commentModel);
        Assert.IsTrue(TryPerformRequest(request, out var response), "Comment post failed " + response.StatusCode + " " + response.Content);
        
        string resourceLocation = (string)response.Headers.First(e => e.Name == "Location")?.Value;
        Match match = utils.Regexes.CreatedId.Match(resourceLocation);
        Assert.IsTrue(match.Success, "Could not match returned post id");
        long commentId = long.Parse(match.Value);
        TestsSetup.TrackInsertedEntries("comment", commentId, 2);
        
        Assert.IsTrue(dbConnection.SelectAndDeserialize(
            "SELECT text, fk_account_id, fk_item_id FROM comment WHERE id = @id",
            new Dictionary<string, object>() { ["id"] = (int)commentId },
            out var results));
        Assert.IsNotEmpty(results);

        var comment = results.First();

        Assert.AreEqual(commentModel.Text, comment["text"]);
        Assert.AreEqual(credentials.Id, comment["fk_account_id"]);
        Assert.AreEqual(postId, comment["fk_item_id"]);
    }

    [Test]
    public void _4_Resource_Accessibility_And_Logout()
    {
        Assert.IsNotEmpty(authorizationToken, "You must be logged in");
        Assert.IsNotNull(authorizationToken, "You must be logged in");

        RestRequest itemListRequest = new RestRequest("/categories/1/items");
        Assert.IsTrue(TryPerformRequest(itemListRequest, out var response), "Item list fetch failed " + response.StatusCode + " " + response.Content);

        RestRequest logoutRequest = new RestRequest("/tokens", Method.Delete);
        Assert.IsTrue(TryPerformRequest(logoutRequest, out response), "Logout failed" + response.StatusCode + " " + response.Content);

        TryPerformRequest(itemListRequest, out response);
        Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized, "Resource should not be accessible when logged out");

        Assert.IsTrue(dbConnection.SelectAndDeserialize(
            "SELECT token FROM blocked_token WHERE token = @token", 
            new(){ ["token"] = authorizationToken },
            out var results));
        Assert.IsNotEmpty(results);
    }

    private bool TryPerformRequest(RestRequest request, out RestResponse response)
    {
        request.AddHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(authorizationToken))
        {
            request.AddHeader("Authorization", $"Bearer {authorizationToken}");
        }

        response = client.Execute(request);
        return response.StatusCode is HttpStatusCode.OK or HttpStatusCode.NoContent or HttpStatusCode.Created;
    }
}