using DAL.Dto;
using DAL.Repository;
using DAL.Schema;
using Dapper.FluentMap;
using FamousQuoteQuiz.Infrastructure;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xunit;

namespace DAL.Tests
{
    public class UserRepositoryIntegrationTests
    {
        private readonly SqlConnection Conn = null;
        private readonly UserRepository Sut = null;
        private readonly ConnectionStringProvider cs = null;

        public UserRepositoryIntegrationTests()
        {
            cs = new ConnectionStringProvider();

            Conn = new SqlConnection(cs.ConnectionString);

            var log = new SeriLogFacility<UserRepository>(Log.Logger);
            Sut = new UserRepository(Conn, log);

            FluentMapper.EntityMaps.Clear();
            FluentMapper.Initialize(cfg => cfg.AddMap(new UserSchema()));
            Conn.Open();
        }

        [Fact]
        public async void CreateReadDelete()
        {
            var testUser = new UserDto
            {
                Password = DateTime.Now.Ticks.ToString(),
                Username = DateTime.Now.Ticks.ToString()
            };

            var createTest = await Sut.Create(new[] { testUser });

            Assert.Equal(1, createTest);

            var test = await Sut.Read();

            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<UserDto>>(test);

            var deleteTest = await Sut.Delete(new[] { test.FirstOrDefault().Id ?? 0});

            Assert.Equal(1, deleteTest);
        }
        [Fact]
        public async void CreateReadUpdateDelete()
        {
            var testUser = new UserDto
            {
                Password = DateTime.Now.Ticks.ToString(),
                Username = DateTime.Now.Ticks.ToString()
            };

            var createTest = await Sut.Create(new[] { testUser });

            Assert.Equal(1, createTest);

            var readTest = await Sut.Read();

            Assert.NotNull(readTest);
            Assert.IsAssignableFrom<IEnumerable<UserDto>>(readTest);

            readTest.FirstOrDefault().Username += "1";
            readTest.FirstOrDefault().Password += "1";

            var test = await Sut.Update(new[] { readTest.FirstOrDefault() });

            Assert.Equal(1, test);

            var deleteTest = await Sut.Delete(new[] { readTest.FirstOrDefault().Id ?? 0 });

            Assert.Equal(1, deleteTest);
        }
    }
}
