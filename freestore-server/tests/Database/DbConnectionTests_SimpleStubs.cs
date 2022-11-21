﻿using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using server.Database;

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
