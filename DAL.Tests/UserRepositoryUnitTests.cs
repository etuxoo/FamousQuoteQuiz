using DAL.Dto;
using DAL.Repository;
using Dapper;
using FamousQuoteQuiz.Infrastructure;
using Moq;
using Moq.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xunit;

namespace DAL.Tests
{
    public class UserRepositoryUnitTests
    {
        private readonly Mock<IDbConnection> ConnMoq = new Mock<IDbConnection>();
        private readonly Mock<IDbCommand> cmdMoq = new Mock<IDbCommand>();
        private readonly Mock<IDbTransaction> transactionMoq = new Mock<IDbTransaction>();
        private readonly Mock<ILogFacility<UserRepository>> LogMoq = new Mock<ILogFacility<UserRepository>>();
        private readonly UserRepository Sut = null;

        public UserRepositoryUnitTests()
        {
            ConnMoq.Setup(db => db.CreateCommand()).Returns(cmdMoq.Object);
            ConnMoq.Setup(db => db.BeginTransaction()).Returns(transactionMoq.Object);
            Sut = new UserRepository(ConnMoq.Object, LogMoq.Object);
        }

        //[Fact]
        // Moq.Dapper do not support mock for ExecuteAsync
        //public async void Create()
        //{
        //    ConnMoq.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
        //        .ReturnsAsync(1)
        //        .Verifiable();

        //    var test = await Sut.Create(new[] { new UserDto(), new UserDto(), new UserDto() });

        //    ConnMoq.Verify();
        //    Assert.Equal(3, test);
        //}

        //[Fact]
        //public async void ReadManyById()
        //{
        //    ConnMoq.SetupDapperAsync(c => c.QueryAsync(It.IsAny<string>() ,null,null,null,null))
        //        .ReturnsAsync(new[] { new UserDto() })
        //        .Verifiable();

        //    var test = await Sut.Read(new[] { 1,2,3,});

        //    ConnMoq.Verify();
        //    Assert.NotNull(test);
        //    Assert.IsAssignableFrom<IEnumerable<UserDto>>(test);

        //    //The Moq.dapper Gives strange behavior here.
        //    // TODO : Check with integration test
        //    Assert.Equal(3, test.ToList().Count);
        //}
    }
}
