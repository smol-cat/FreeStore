using NUnit.Framework;
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
    public void Test1()
    {
        Assert.Pass();
    }
}