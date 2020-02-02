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
    public class QuoteRepositoryUnitTests
    {
        private readonly Mock<IDbConnection> ConnMoq = new Mock<IDbConnection>();
        private readonly Mock<IDbCommand> cmdMoq = new Mock<IDbCommand>();
        private readonly Mock<IDbTransaction> transactionMoq = new Mock<IDbTransaction>();
        private readonly Mock<ILogFacility<QuoteRepository>> LogMoq = new Mock<ILogFacility<QuoteRepository>>();
        private readonly QuoteRepository Sut = null;

        public QuoteRepositoryUnitTests()
        {
            ConnMoq.Setup(db => db.CreateCommand()).Returns(cmdMoq.Object);
            ConnMoq.Setup(db => db.BeginTransaction()).Returns(transactionMoq.Object);
            Sut = new QuoteRepository(ConnMoq.Object, LogMoq.Object);
        }

        [Fact]
        public void UpdateFailWithNullCollection()
        {
            Assert.ThrowsAsync<ArgumentException>(() => Sut.Update(null));
        }

        [Fact]
        public void UpdateFailWithEmptyCollection()
        {
            var collection = new List<QuoteDto>();
            Assert.ThrowsAsync<ArgumentException>(() => Sut.Update(collection));
        }

        [Fact]
        public void DeleteFailWithNullCollection()
        {
            Assert.ThrowsAsync<ArgumentException>(() => Sut.Delete(null));
        }
    }
}
