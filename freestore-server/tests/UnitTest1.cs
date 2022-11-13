using Xunit;
using server.Database;
using System;

namespace tests;

public class UnitTest1
{
    [Fact]
    public void DatabaseConnectionTest()
    {
        try
        {
            var dbConnection = new DbConnection($"server={Globals.Host};port={Globals.Port};database={Globals.Database};username={Globals.Username};password={Globals.Password};");
            Assert.True(true, "Database has connected");
        }
        catch (Exception)
        {
            Assert.True(false, "Database failed to connect");
        }
    }
}