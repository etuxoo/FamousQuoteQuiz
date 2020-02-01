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
    public class AuthorRepository : IRepository<AuthorDto>
    {
        private readonly IDbConnection Connection = null;
        private readonly ILogFacility<AuthorRepository> Log = null;

        public AuthorRepository(IDbConnection conn, ILogFacility<AuthorRepository> log)
        {
            Connection = conn;
            Log = log;
        }

        public Task<int> Create(IEnumerable<AuthorDto> records)
        {
            Log.Trace($"{nameof(AuthorRepository)}.{nameof(Create)} has been invoked.");

            Log.Trace("Preparing SQL statement...");


            Task<int> result = null;
            if (records != null)
            {
                var sql = @$"INSERT INTO Authors ( Author )
VALUES (@{nameof(AuthorDto.Author)})
";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                using var transaction = Connection.BeginTransaction();
                result = Connection.ExecuteAsync(sql, records, transaction);
                transaction.Commit();
            }
            else
            {
                Log.Error($"The collection passed to {nameof(AuthorRepository)}.{nameof(Create)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(AuthorRepository)}.{nameof(Create)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(AuthorRepository)}.{nameof(Create)} method completed.");

            return result;
        }

        public Task<int> Delete(IEnumerable<int> ids)
        {
            Log.Trace($"{nameof(AuthorRepository)}.{nameof(Delete)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            Task<int> result = null;

            if (ids != null)
            {

                var sql = "DELETE FROM Authors";

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
                Log.Error($"The collection passed to {nameof(AuthorRepository)}.{nameof(Delete)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(AuthorRepository)}.{nameof(Delete)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(AuthorRepository)}.{nameof(Delete)} method completed.");

            return result;
        }

        public Task<IEnumerable<AuthorDto>> Read(IEnumerable<int> ids = null)
        {
            Log.Trace($"{nameof(AuthorRepository)}.{nameof(Read)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var sql = @"SELECT Id, Author
FROM Authors
";

            if (ids?.Any() ?? false)
            {
                var idList = ids.Aggregate(string.Empty, (text, id) => text += $"'{id}',")
                    .TrimEnd(',')
                    .Replace(",", ", ")
                ;

                sql += $"WHERE Id IN({idList})";

                Log.Debug($"{nameof(AuthorRepository)}.{nameof(Read)}(id = {ids.Count()}) query start. ");
            }

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.QueryAsync<AuthorDto>(sql);

            Log.Debug($"{ids?.Count() ?? 0} authors records retrieved.");

            Log.Trace($"{nameof(AuthorRepository)}.{nameof(Read)} execution completed.");

            return result;
        }

        public Task<int> Update(IEnumerable<AuthorDto> records)
        {
            Log.Trace($"{nameof(AuthorRepository)}.{nameof(Update)} has been invoked.");


            if (!records?.Any() ?? true)
            {
                Log.Error($"No gifts to update.");
                throw new ArgumentException("No gifts to update.");
            }

            Log.Trace("Preparing SQL statement...");

            var sql = $@"UPDATE Authors
SET  Author = @{nameof(AuthorDto.Author)}
WHERE Id = @{nameof(AuthorDto.Id)}";

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.ExecuteAsync(sql, records);

            Log.Debug($"{records} gift records updated.");

            Log.Trace($"{nameof(AuthorRepository)}.{nameof(Update)} execution completed.");

            return result;
        }
    }
}
