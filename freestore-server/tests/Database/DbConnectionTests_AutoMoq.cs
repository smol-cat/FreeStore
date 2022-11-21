using AutoMoq;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using server.Database;

namespace tests.Database
{
    [TestFixture]
    public class DbConnectionTests_AutoMoq
    {
        [Test]
        public void Execute_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mocker = new AutoMoqer();
            var dbConnection = mocker.Create<DbConnection>();
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
            var mocker = new AutoMoqer();
            var dbConnection = mocker.Create<DbConnection>();
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
            var mocker = new AutoMoqer();
            var dbConnection = mocker.Create<DbConnection>();
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
