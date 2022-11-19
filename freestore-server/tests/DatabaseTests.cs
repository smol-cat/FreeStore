using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using server.Database;
using server.Models.Category;
using tests.utils;

namespace tests;

public class DatabaseTests
{
    private DbConnection dbConnection = TestsSetup.Db;
    
    /*[SetUp]
    public void Setup()
    {
        dbConnection = new DbConnection(new TestConfiguration());
        insertedEntries = new();
    }

    [TearDown]
    public void TearDown()
    {
        foreach (var entry in insertedEntries)
        {
            entry.Value.ForEach(id => dbConnection.Execute($"DELETE FROM {entry.Key} WHERE id = @id", new () { ["id"] = (int)id }));
        }
        
        insertedEntries = new();
    }*/
    
    [Test]
    public void Bad_Sql_Returns_False_With_Exception()
    {
        int lastException = dbConnection.LastException?.GetHashCode() ?? -1;
        bool success = dbConnection.Execute("SELECT * FROM", new());
        Assert.IsTrue(!success && dbConnection.LastException.GetHashCode() != lastException);
    }

    [Test]
    [TestCase("SELECT * FROM category", new string[] {}, new object[] {})]
    [TestCase("SELECT * FROM category WHERE id = @id", new string[] { "id" }, new object[] { 1 })]
    public void Good_Sql_Returns_True(string query, string[] keys, object[] values)
    {
        Dictionary<string, object> parameters = keys.Zip(values).ToDictionary(e => e.First, e => e.Second);
        bool success = dbConnection.Execute(query, parameters);
        Assert.IsTrue(success);
    }

    [Test]
    public void Inserted_Entries_Return_Last_Inserted_Ids()
    {
        long lastInserted = dbConnection.LastInsertedId;
        
        if(!dbConnection.Execute(
               @"INSERT INTO category (name, description) VALUES (""test name"", ""test description"")", new()))
        {
            Assert.Fail("Failed to insert into database");
        }
        
        long firstId = dbConnection.LastInsertedId;
        TestsSetup.TrackInsertedEntries("category", firstId);

        if (!dbConnection.Execute(
                @"INSERT INTO category (name, description) VALUES (""test name"", ""test description"")", new()))
        {
            Assert.Fail("Failed to insert into database");
        }

        long secondId = dbConnection.LastInsertedId;
        TestsSetup.TrackInsertedEntries("category", secondId);

        Assert.AreNotEqual(firstId, lastInserted);
        Assert.AreNotEqual(secondId, lastInserted);
        Assert.AreEqual(secondId - firstId, 1);
    }

    [Test]
    public void Disconnected_Conn_Reconnects_And_Executes_Query()
    {
        FieldInfo connectionField = typeof(DbConnection).GetField("conn", BindingFlags.NonPublic | BindingFlags.Static);
        MySqlConnection connection = (MySqlConnection)connectionField.GetValue(dbConnection);
        connection.Close();
        bool success = dbConnection.Execute("SELECT * FROM category", new(){});
        Assert.IsTrue(success);
    }

    [Test]
    [TestCase($"SELECT * FROM category WHERE id = @id AND unlisted = 0", new string[] { "id" }, new object[] { 1 })]
    [TestCase($"SELECT * FROM category", new string[] { }, new object[] {})]
    public void Selected_Deserializable_Categories_Are_Filled(string query, string[] keys, object[] values)
    {
        Dictionary<string, object> parameters = keys.Zip(values).ToDictionary(e => e.First, e => e.Second);
        bool success = dbConnection.SelectAndDeserialize<IdCategoryModel>(query,
            parameters, out List<IdCategoryModel> categories);
        
        Assert.IsTrue(success);
        Assert.IsTrue(categories.Any());
        Assert.IsTrue(categories.All(e => e.Id != default(int) && 
                                          !string.IsNullOrEmpty(e.Name) && 
                                          !string.IsNullOrEmpty(e.Description)));
    }
    
    [Test]
    [TestCase($"SELECT * FROM category WHERE id = @id AND unlisted = 0", new string[] { "id" }, new object[] { 1 })]
    [TestCase($"SELECT * FROM category", new string[] { }, new object[] {})]
    public void Disconnected_Conn_Reconnects_And_Select_Deserializable(string query, string[] keys, object[] values)
    {
        FieldInfo connectionField = typeof(DbConnection).GetField("conn", BindingFlags.NonPublic | BindingFlags.Static);
        MySqlConnection connection = (MySqlConnection)connectionField.GetValue(dbConnection);
        connection.Close();
        Selected_Deserializable_Categories_Are_Filled(query, keys, values);
    }

    [Test]
    [TestCase("SELCT * FROM category")]
    [TestCase("SELECT * FROM category WHERE id = @id")]
    public void Select_Deserializable_Bad_Sql_Returns_False(string query)
    {
        bool success = dbConnection.SelectAndDeserialize<IdCategoryModel>(query,
            new Dictionary<string, object>(), out List<IdCategoryModel> categories);
        Assert.IsFalse(success);
    }

    [Test]
    [TestCase("SELECT id, name FROM category", new string[0], new object[0]  ,new string[] { "id", "name"})]
    [TestCase("SELECT id, price, title FROM item WHERE fk_category_id = @id", new string[] { "id" }, new object[] { 1 }, new string[] { "id", "price", "title"})]
    [TestCase("SELECT text, date_created FROM comment", new string[0], new object[0], new string[] { "text", "date_created" })]
    [TestCase($"SELECT id, is_blocked FROM account WHERE id = @id AND is_blocked = @blocked", new string[] { "id", "blocked" }, new object[] { 1, 0 }, new string[] { "id", "is_blocked" })]
    public void Select_To_Dictionary_Contains_Requested_Entries(string query, string[] keys, object[] values, string[] expectedKeys)
    {
        Dictionary<string, object> parameters = keys.Zip(values).ToDictionary(e => e.First, e => e.Second);
        bool success = dbConnection.SelectAndDeserialize(query, parameters, out var results);
        Assert.IsTrue(success && expectedKeys.All(e => results.First().ContainsKey(e)));
    }

    [Test]
    [TestCase("SELCT * FROM category")]
    [TestCase("SELECT * FROM category WHERE id = @id")]
    public void Select_To_Dictionary_Bad_Sql_Returns_False(string query)
    {
        bool success = dbConnection.SelectAndDeserialize(query,
            new Dictionary<string, object>(), out var results);
        Assert.IsFalse(success);
    }

    [Test]
    [TestCase("SELECT id, name FROM category", new string[0], new object[0]  ,new string[] { "id", "name"})]
    [TestCase("SELECT id, price, title FROM item WHERE fk_category_id = @id", new string[] { "id" }, new object[] { 1 }, new string[] { "id", "price", "title"})]
    [TestCase("SELECT text, date_created FROM comment", new string[0], new object[0], new string[] { "text", "date_created" })]
    [TestCase($"SELECT id, is_blocked FROM account WHERE id = @id AND is_blocked = @blocked", new string[] { "id", "blocked" }, new object[] { 1, 0 }, new string[] { "id", "is_blocked" })]
    public void Disconnected_Conn_Reconnects_And_Select_To_Dictionary(string query, string[] keys, object[] values, string[] expectedKeys)
    {
        FieldInfo connectionField = typeof(DbConnection).GetField("conn", BindingFlags.NonPublic | BindingFlags.Static);
        MySqlConnection connection = (MySqlConnection)connectionField.GetValue(dbConnection);
        connection.Close();
        Select_To_Dictionary_Contains_Requested_Entries(query, keys, values, expectedKeys);
    }
}