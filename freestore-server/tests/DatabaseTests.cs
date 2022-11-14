using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Org.BouncyCastle.Asn1.Cms;
using server.Database;

namespace tests;

public class DatabaseTests
{
    private DbConnection dbConnection;
    
    [SetUp]
    public void Setup()
    {
        dbConnection = new DbConnection($"server={Globals.Host};port={Globals.Port};database={Globals.Database};username={Globals.Username};password={Globals.Password};");
    }

    [Test]
    public void Bad_Sql_Returns_False()
    {
        bool success = dbConnection.Execute("SELECT * FROM", new());
        Assert.IsTrue(!success);
    }

    [Test]
    [TestCase("SELECT * FROM category", new string[] {}, new object[] {})]
    [TestCase("SELECT * FROM category WHERE id = @id", new string[] { "id" }, new object[] { 1 })]
    public void Good_Sql_Returns_True(string query, string[] keys, object[] values)
    {
        Dictionary<string, object> parameters = keys.Zip(values).ToDictionary(e => e.First, e => e.Second);
        bool success = dbConnection.Execute("SELECT * FROM category", parameters);
        Assert.IsTrue(success);
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
}