using Dapper;
using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Metrics.DotNet.Samples.Services.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Services.Repository
{
    public class PostgresBookRepository : IPostgresBookRepository
    {
        private readonly IOptions<PostgresSettings> _settings;

        public PostgresBookRepository(IOptions<PostgresSettings> settings)
        {
            _settings = settings;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
            await connection.OpenAsync();

            var command = new CommandDefinition("SELECT * FROM books");
            return await connection.QueryAsync<Book>(command);
        }

        public async Task<Book> GetBook(Guid id)
        {
            using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
            await connection.OpenAsync();

            var command = new CommandDefinition("SELECT * FROM books WHERE id = @id", new { id });
            return await connection.QueryFirstAsync<Book>(command);
        }

        public Task SetBook(Book book)
        {
            throw new NotImplementedException();
        }

        public Task SetBooks(List<Book> book)
        {
            throw new NotImplementedException();
        }
    }
}
