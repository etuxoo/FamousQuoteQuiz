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
    public class QuoteRepositoryIntegrationTests
    {
        private readonly SqlConnection Conn = null;
        private readonly QuoteRepository Sut = null;
        private readonly ConnectionStringProvider cs = null;

        public QuoteRepositoryIntegrationTests()
        {
            cs = new ConnectionStringProvider();

            Conn = new SqlConnection(cs.ConnectionString);

            var log = new SeriLogFacility<QuoteRepository>(Log.Logger);
            Sut = new QuoteRepository(Conn, log);

            FluentMapper.EntityMaps.Clear();
            FluentMapper.Initialize(cfg => cfg.AddMap(new QuoteSchema()));
            Conn.Open();
        }

        [Fact]
        public async void CreateReadDelete()
        {
            var testQuote = new QuoteDto
            {
                Quote = DateTime.Now.Ticks.ToString(),
                AuthorId = 1
            };

            var createTest = await Sut.Create(new[] { testQuote });

            Assert.Equal(1, createTest);

            var test = await Sut.Read();

            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<QuoteDto>>(test);

            var deleteTest = await Sut.Delete(new[] { test.FirstOrDefault().Id ?? 0 });

            Assert.Equal(1, deleteTest);
        }
        [Fact]
        public async void CreateReadUpdateReadDelete()
        {
            var testQuote = new QuoteDto
            {
                Quote = DateTime.Now.Ticks.ToString(),
                AuthorId = 1
            };

            var createTest = await Sut.Create(new[] { testQuote });

            Assert.Equal(1, createTest);

            var readTest = await Sut.Read();

            Assert.NotNull(readTest);
            Assert.IsAssignableFrom<IEnumerable<QuoteDto>>(readTest);

            readTest.FirstOrDefault().Quote += "1";
            readTest.FirstOrDefault().AuthorId += 1;

            var test = await Sut.Update(new[] { readTest.FirstOrDefault() });

            Assert.Equal(1, test);

            var secondReadTest = await Sut.Read();

            var checkValues = secondReadTest.FirstOrDefault().Quote == readTest.FirstOrDefault().Quote && secondReadTest.FirstOrDefault().AuthorId == readTest.FirstOrDefault().AuthorId;

            Assert.True(checkValues);

            var deleteTest = await Sut.Delete(new[] { readTest.FirstOrDefault().Id ?? 0 });

            Assert.Equal(1, deleteTest);
        }
    }
}
