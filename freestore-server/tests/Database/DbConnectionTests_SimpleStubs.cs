using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using server.Database;
using server.Models.Account;
using tests.utils;

namespace tests.Database
{
    [TestFixture]
    public class DbConnectionTests_SimpleStubs
    {
        private StubIConfiguration stubConfiguration;

        [SetUp]
        public void SetUp()
        {
            this.stubConfiguration = new StubIConfiguration();
        }

        private DbConnection CreateDbConnection()
        {
            return new DbConnection(
                this.stubConfiguration);
        }

        [Test]
        public void Execute_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dbConnection = this.CreateDbConnection();
            string query = "SELECT * FROM category";
            Dictionary<string, object> parameters = new();

            // Act
            var result = dbConnection.Execute(
                query,
                parameters);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void SelectAndDeserialize_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dbConnection = this.CreateDbConnection();
            string query = "SELECT * FROM account WHERE id = @id AND is_blocked = 0";
            Dictionary<string, object> parameters = new() { ["id"] = 1 };
            List<AccountModel> results = null;

            // Act
            var result = dbConnection.SelectAndDeserialize<AccountModel>(
                query,
                parameters,
                out results);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotEmpty(results);
        }

        [Test]
        public void SelectAndDeserialize_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var dbConnection = this.CreateDbConnection();
            string query = "SELECT id, name, last_name FROM account WHERE id = @id AND is_blocked = 0";
            Dictionary<string, object> parameters = new() { ["id"] = 1 };
            List<Dictionary<string, object>> results = null;

            // Act
            var result = dbConnection.SelectAndDeserialize(
                query,
                parameters,
                out results);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(results.First().ContainsKey("id"));
            Assert.IsTrue(results.First().ContainsKey("name"));
            Assert.IsTrue(results.First().ContainsKey("last_name"));
        }
    }
}
