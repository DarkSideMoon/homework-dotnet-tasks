﻿using Dapper;
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
                using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                var command = new CommandDefinition("SELECT * FROM books");
                return await connection.QueryAsync<Book>(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Book> GetBook(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                var command = new CommandDefinition("SELECT * FROM books WHERE id = @id", new { id });
                return await connection.QueryFirstAsync<Book>(command);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task SetBook(Book book)
        {
            try
            {
                using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                var command = new CommandDefinition("INSERT INTO public.book(\"AuthorFirstName\", \"AuthorLastName\", \"BookType\", \"CountOfPages\", \"ISBN\", \"Id\", \"Language\", \"Price\", \"Title\", \"AuthorEmail\") " +
                    "VALUES(@AuthorFirstName, @AuthorLastName, @BookType, @CountOfPages, @ISBN, @Id, @Language, @Price, @Title, @AuthorEmail);",
                    new
                    {
                        book.AuthorFirstName,
                        book.AuthorLastName,
                        book.BookType,
                        book.CountOfPages,
                        book.ISBN,
                        book.Id,
                        book.Language,
                        book.Price,
                        book.Title,
                        book.AuthorEmail
                    });
                await connection.ExecuteAsync(command);
            }
            catch (Exception ex)
            {
                throw ex;
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
                using var connection = new NpgsqlConnection(_settings.Value.ConnectionString);
                await connection.OpenAsync();

                var sqlText = books.Aggregate(new StringBuilder(), (sb, book) => sb.AppendLine(
                "INSERT INTO public.book(\"AuthorFirstName\", \"AuthorLastName\", \"BookType\", \"CountOfPages\", \"ISBN\", \"Id\", \"Language\", \"Price\", \"Title\", \"AuthorEmail\") " +
                    $"VALUES('{book.AuthorFirstName}', '{book.AuthorLastName}', '{book.BookType}', '{book.CountOfPages}', " +
                    $"'{book.ISBN}', '{book.Id}', '{book.Language}', {book.Price}, '{book.Title}', '{book.AuthorEmail}'); "));

                var sql = sqlText.ToString();
                await connection.ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
