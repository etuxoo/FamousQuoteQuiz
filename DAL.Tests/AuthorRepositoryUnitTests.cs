using DAL.Dto;
using DAL.Repository;
using FamousQuoteQuiz.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace DAL.Tests
{
    public class AuthorRepositoryUnitTests
    {
        private readonly Mock<IDbConnection> ConnMoq = new Mock<IDbConnection>();
        private readonly Mock<IDbCommand> cmdMoq = new Mock<IDbCommand>();
        private readonly Mock<IDbTransaction> transactionMoq = new Mock<IDbTransaction>();
        private readonly Mock<ILogFacility<AuthorRepository>> LogMoq = new Mock<ILogFacility<AuthorRepository>>();
        private readonly AuthorRepository Sut = null;

        public AuthorRepositoryUnitTests()
        {
            ConnMoq.Setup(db => db.CreateCommand()).Returns(cmdMoq.Object);
            ConnMoq.Setup(db => db.BeginTransaction()).Returns(transactionMoq.Object);
            Sut = new AuthorRepository(ConnMoq.Object, LogMoq.Object);
        }

        [Fact]
        public void UpdateFailWithNullCollection()
        {
            Assert.ThrowsAsync<ArgumentException>(() => Sut.Update(null));
        }

        [Fact]
        public void UpdateFailWithEmptyCollection()
        {
            var collection = new List<AuthorDto>();
            Assert.ThrowsAsync<ArgumentException>(() => Sut.Update(collection));
        }

        [Fact]
        public void DeleteFailWithNullCollection()
        {
            Assert.ThrowsAsync<ArgumentException>(() => Sut.Delete(null));
        }
    }
}
