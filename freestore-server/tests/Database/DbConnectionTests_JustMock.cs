using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using Telerik.JustMock;
using server.Database;

namespace tests.Database
{
    [TestFixture]
    public class DbConnectionTests_JustMock
    {
        private IConfiguration mockConfiguration;

        [SetUp]
        public void SetUp()
        {
            this.mockConfiguration = Mock.Create<IConfiguration>();
        }

        private DbConnection CreateDbConnection()
        {
            return new DbConnection(
                this.mockConfiguration);
        }

        [Test]
        public void Execute_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dbConnection = this.CreateDbConnection();
            string query = null;
            Dictionary parameters = null;

            // Act
            var result = dbConnection.Execute(
                query,
                parameters);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void SelectAndDeserialize_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dbConnection = this.CreateDbConnection();
            string query = null;
            Dictionary parameters = null;
            List result = null;

            // Act
            var result = dbConnection.SelectAndDeserialize(
                query,
                parameters,
                out result);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void SelectAndDeserialize_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var dbConnection = this.CreateDbConnection();
            string query = null;
            Dictionary parameters = null;
            List result = null;

            // Act
            var result = dbConnection.SelectAndDeserialize(
                query,
                parameters,
                out result);

            // Assert
            Assert.Fail();
        }
    }
}
