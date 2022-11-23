using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RestSharp;
using server.Database;
using tests.utils;
namespace tests;

[SetUpFixture]
public class TestsSetup
{
    public static RestClient Client;
    public static DbConnection Db;
    private static Dictionary<string, DbTableEntries> InsertedEntries;
        
    [OneTimeSetUp]
    public void Setup()
    {
        Db = new DbConnection(new StubIConfiguration());
        Client = new RestClient(Globals.ApiRoot);
        InsertedEntries = new();
    }
        
    [OneTimeTearDown]
    public void TearDown()
    {
        List<KeyValuePair<string, DbTableEntries>> orderedEntries =
            InsertedEntries.OrderByDescending(e => e.Value.Priority).ToList();
        
        foreach (var entry in orderedEntries)
        {
            entry.Value.Ids.ForEach(id => Db.Execute($"DELETE FROM {entry.Key} WHERE id = @id", new () { ["id"] = (int)id }));
        }
        
        InsertedEntries = new();
    }
    
    public static void TrackInsertedEntries(string table, long identifier, int priority = 0)
    {
        InsertedEntries.TryAdd(table, new DbTableEntries());
        InsertedEntries[table].Priority = priority;
        InsertedEntries[table].Ids.Add(identifier);
    }

    private class DbTableEntries
    {
        public int Priority;
        public List<long> Ids = new List<long>();
    }
}