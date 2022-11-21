using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using server.Database;

namespace tests.Database
{
    [TestFixture]
    public class DbConnectionTests_Moq
    {
        private MockRepository mockRepository;

        private Mock<IConfiguration> mockConfiguration;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
        }

        private DbConnection CreateDbConnection()
        {
            return new DbConnection(
                this.mockConfiguration.Object);
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
            this.mockRepository.VerifyAll();
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
            this.mockRepository.VerifyAll();
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
            this.mockRepository.VerifyAll();
        }
    }
}
