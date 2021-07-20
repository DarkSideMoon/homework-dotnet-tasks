using Dapper;
using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Metrics.DotNet.Samples.Services.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            try
            {
                await using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                // TODO: Add pagination to get all data
                var command = new CommandDefinition("SELECT * FROM public.book");
                return await connection.QueryAsync<Book>(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Book>> GetBooks(int count)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                var command = new CommandDefinition("SELECT * FROM public.book where random() < 0.01 LIMIT @count", new { count });
                return await connection.QueryAsync<Book>(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Book> GetBook(int id)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                var command = new CommandDefinition("SELECT * FROM public.book WHERE id = @id", new { id });
                return await connection.QueryFirstAsync<Book>(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SetBook(Book book)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                var command = new CommandDefinition("INSERT INTO public.book(\"AuthorFirstName\", \"AuthorLastName\", \"BookType\", \"CountOfPages\", \"ISBN\", \"Language\", \"Price\", \"Title\", \"AuthorEmail\") " +
                    "VALUES(@AuthorFirstName, @AuthorLastName, @BookType, @CountOfPages, @ISBN, @Language, @Price, @Title, @AuthorEmail);",
                    new
                    {
                        book.AuthorFirstName,
                        book.AuthorLastName,
                        book.BookType,
                        book.CountOfPages,
                        book.ISBN,
                        book.Language,
                        book.Price,
                        book.Title,
                        book.AuthorEmail
                    });
                await connection.ExecuteAsync(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SetBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                await SetBook(book);
            }
        }

        public async Task BulkSetBooks(List<Book> books)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                var sqlText = books.Aggregate(new StringBuilder(), (sb, book) => sb.AppendLine(
                "INSERT INTO public.book(\"AuthorFirstName\", \"AuthorLastName\", \"BookType\", \"CountOfPages\", \"ISBN\", \"Id\", \"Language\", \"Price\", \"Title\", \"AuthorEmail\") " +
                    $"VALUES('{book.AuthorFirstName}', '{book.AuthorLastName}', '{book.BookType}', '{book.CountOfPages}', " +
                    $"'{book.ISBN}', '{book.Id}', '{book.Language}', {book.Price}, '{book.Title}', '{book.AuthorEmail}'); "));

                var sql = sqlText.ToString();
                await connection.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Book> GetRandomBook()
        {
            try
            {
                await using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                var command = new CommandDefinition("select * from public.book where random() < 0.01 LIMIT 1;");
                return await connection.QueryFirstAsync<Book>(command);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
