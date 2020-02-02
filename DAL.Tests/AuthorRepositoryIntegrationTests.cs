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
    public class AuthorRepositoryIntegrationTests
    {
        private readonly SqlConnection Conn = null;
        private readonly AuthorRepository Sut = null;
        private readonly ConnectionStringProvider cs = null;

        public AuthorRepositoryIntegrationTests()
        {
            cs = new ConnectionStringProvider();

            Conn = new SqlConnection(cs.ConnectionString);

            var log = new SeriLogFacility<AuthorRepository>(Log.Logger);
            Sut = new AuthorRepository(Conn, log);

            FluentMapper.EntityMaps.Clear();
            FluentMapper.Initialize(cfg => cfg.AddMap(new AuthorSchema()));
            Conn.Open();
        }

        [Fact]
        public async void CreateReadDelete()
        {
            var testAuthor = new AuthorDto
            {
                 Author = DateTime.Now.Ticks.ToString()
            };

            var createTest = await Sut.Create(new[] { testAuthor });

            Assert.Equal(1, createTest);

            var test = await Sut.Read(new[] { testAuthor.Author});

            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<AuthorDto>>(test);

            var deleteTest = await Sut.Delete(new[] { test.FirstOrDefault().Id ?? 0 });

            Assert.Equal(1, deleteTest);
        }
        [Fact]
        public async void CreateReadUpdateReadDelete()
        {
            var testAuthor = new AuthorDto
            {
                Author = DateTime.Now.Ticks.ToString()
            };

            var createTest = await Sut.Create(new[] { testAuthor });

            Assert.Equal(1, createTest);

            var readTest = await Sut.Read(new[] { testAuthor.Author });

            Assert.NotNull(readTest);
            Assert.IsAssignableFrom<IEnumerable<AuthorDto>>(readTest);

            readTest.FirstOrDefault().Author += "1";

            var test = await Sut.Update(new[] { readTest.FirstOrDefault() });

            Assert.Equal(1, test);

            var secondReadTest = await Sut.Read(new[] { readTest.FirstOrDefault().Author });

            var checkValues = secondReadTest.FirstOrDefault().Author == readTest.FirstOrDefault().Author;

            Assert.True(checkValues);

            var deleteTest = await Sut.Delete(new[] { readTest.FirstOrDefault().Id ?? 0 });

            Assert.Equal(1, deleteTest);
        }
    }
}
