using DAL.Dto;
using Dapper;
using FamousQuoteQuiz.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class QuoteRepository : IRepository<QuoteDto>
    {

        private readonly IDbConnection Connection = null;
        private readonly ILogFacility<QuoteRepository> Log = null;

        public QuoteRepository(IDbConnection conn, ILogFacility<QuoteRepository> log)
        {
            Connection = conn;
            Log = log;
        }

        public Task<int> Create(IEnumerable<QuoteDto> records)
        {
            Log.Trace($"{nameof(QuoteRepository)}.{nameof(Create)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            Task<int> result = null;

            if (records != null)
            {
                var sql = @$"INSERT INTO Users ( Quote, AuthorId )
VALUES (@{nameof(QuoteDto.Quote)}, @{nameof(QuoteDto.AuthorId)})
";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                using var transaction = Connection.BeginTransaction();
                result = Connection.ExecuteAsync(sql, records, transaction);
                transaction.Commit();
            }
            else
            {
                Log.Error($"The collection passed to {nameof(QuoteRepository)}.{nameof(Create)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(QuoteRepository)}.{nameof(Create)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(QuoteRepository)}.{nameof(Create)} method completed.");

            return result;
        }

        public Task<int> Delete(IEnumerable<int> ids)
        {
            Log.Trace($"{nameof(QuoteRepository)}.{nameof(Delete)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            Task<int> result = null;

            if (ids != null)
            {

                var sql = "DELETE FROM Quotes";

                var idList = ids.Aggregate(string.Empty, (text, id) => text += $"'{id}',")
                        .TrimEnd(',')
                        .Replace(",", ", ");

                sql += $" WHERE Id IN({idList})";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                using var transaction = Connection.BeginTransaction();
                result = Connection.ExecuteAsync(sql, ids, transaction);
                transaction.Commit();
            }
            else
            {
                Log.Error($"The collection passed to {nameof(QuoteRepository)}.{nameof(Delete)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(QuoteRepository)}.{nameof(Delete)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(QuoteRepository)}.{nameof(Delete)} method completed.");

            return result;
        }

        public Task<IEnumerable<QuoteDto>> Read(IEnumerable<int> ids=null)
        {
            Log.Trace($"{nameof(QuoteRepository)}.{nameof(Read)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var sql = @"SELECT Id, Quote, AuthorId
FROM Quotes
";

            if (ids?.Any() ?? false)
            {
                var idList = ids.Aggregate(string.Empty, (text, id) => text += $"'{id}',")
                    .TrimEnd(',')
                    .Replace(",", ", ")
                ;

                sql += $"WHERE Id IN({idList})";

                Log.Debug($"{nameof(QuoteRepository)}.{nameof(Read)}(id = {ids.Count()}) query start. ");
            }

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.QueryAsync<QuoteDto>(sql);

            Log.Debug($"{ids?.Count() ?? 0} quote records retrieved.");

            Log.Trace($"{nameof(QuoteRepository)}.{nameof(Read)} execution completed.");

            return result;
        }

        public Task<int> Update(IEnumerable<QuoteDto> records)
        {
            Log.Trace($"{nameof(QuoteRepository)}.{nameof(Update)} has been invoked.");


            if (!records?.Any() ?? true)
            {
                Log.Error($"No gifts to update.");
                throw new ArgumentException("No gifts to update.");
            }

            Log.Trace("Preparing SQL statement...");

            var sql = $@"UPDATE Gifts
SET  Quote = @{nameof(QuoteDto.Quote)}, AuthorId = @{nameof(QuoteDto.AuthorId)}
WHERE Id = @{nameof(QuoteDto.Id)}";

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.ExecuteAsync(sql, records);

            Log.Debug($"{records} gift records updated.");

            Log.Trace($"{nameof(QuoteRepository)}.{nameof(Update)} execution completed.");

            return result;
        }
    }
}
