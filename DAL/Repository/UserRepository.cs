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
    public class UserRepository : IRepository<UserDto>
    {
        private readonly IDbConnection Connection = null;
        private readonly ILogFacility<UserRepository> Log = null;

        public UserRepository(IDbConnection conn, ILogFacility<UserRepository> log)
        {
            Connection = conn;
            Log = log;
        }

        public Task<int> Create(IEnumerable<UserDto> records)
        {
            Log.Trace($"{nameof(UserRepository)}.{nameof(Create)} has been invoked.");

            Log.Trace("Preparing SQL statement...");


            Task<int> result = null;
            if (records != null)
            {
                var sql = @$"INSERT INTO Users (Username, Password)
VALUES (@{nameof(UserDto.Username)}, @{nameof(UserDto.Password)})
";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                result = Connection.ExecuteAsync(sql, records);
            }
            else
            {
                Log.Error($"The collection passed to {nameof(UserRepository)}.{nameof(Create)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(UserRepository)}.{nameof(Create)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(UserRepository)}.{nameof(Create)} method completed.");

            return result;
        }

        public Task<int> Delete(IEnumerable<int> ids)
        {
            Log.Trace($"{nameof(UserRepository)}.{nameof(Delete)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            Task<int> result = null;

            if (ids != null)
            {

                var sql = "DELETE FROM Users";

                var idList = ids.Aggregate(string.Empty, (text, id) => text += $"'{id}',")
                        .TrimEnd(',')
                        .Replace(",", ", ");

                sql += $" WHERE Id IN({idList})";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                result = Connection.ExecuteAsync(sql, ids);
            }
            else
            {
                Log.Error($"The collection passed to {nameof(UserRepository)}.{nameof(Delete)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(UserRepository)}.{nameof(Delete)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(UserRepository)}.{nameof(Delete)} method completed.");

            return result;
        }

        public Task<IEnumerable<UserDto>> Read(IEnumerable<int> ids = null)
        {
            Log.Trace($"{nameof(UserRepository)}.{nameof(Read)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var sql = @"SELECT Id, Username, Password
FROM Users
";

            if (ids?.Any() ?? false)
            {
                var idList = ids.Aggregate(string.Empty, (text, id) => text += $"'{id}',")
                    .TrimEnd(',')
                    .Replace(",", ", ")
                ;

                sql += $"WHERE Id IN({idList})";

                Log.Debug($"{nameof(UserRepository)}.{nameof(Read)}(id = {ids.Count()}) query start. ");
            }

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.QueryAsync<UserDto>(sql);

            Log.Debug($"{ids?.Count() ?? 0} user records retrieved.");

            Log.Trace($"{nameof(UserRepository)}.{nameof(Read)} execution completed.");

            return result;
        }

        public Task<int> Update(IEnumerable<UserDto> records)
        {
            Log.Trace($"{nameof(UserRepository)}.{nameof(Update)} has been invoked.");


            if (!records?.Any() ?? true)
            {
                Log.Error($"No users to update.");
                throw new ArgumentException("No users to update.");
            }

            Log.Trace("Preparing SQL statement...");

            var sql = $@"UPDATE Users
SET  Username = @{nameof(UserDto.Username)}, Password = @{nameof(UserDto.Password)}
WHERE Id = @{nameof(UserDto.Id)}";

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.ExecuteAsync(sql, records);

            Log.Debug($"{records} gift records updated.");

            Log.Trace($"{nameof(UserRepository)}.{nameof(Update)} execution completed.");

            return result;
        }
    }
}
