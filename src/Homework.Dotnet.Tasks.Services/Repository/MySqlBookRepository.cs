using Homework.Dotnet.Tasks.Contracts;
using Homework.Dotnet.Tasks.Services.Repository.Interfaces;
using Homework.Dotnet.Tasks.Services.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace Homework.Dotnet.Tasks.Services.Repository
{
    public class MySqlBookRepository : IMySqlBookRepository
    {
        private readonly IOptions<MySqlSettings> _settings;

        public MySqlBookRepository(IOptions<MySqlSettings> settings)
        {
            _settings = settings;
        }

        public Task<Book> GetBook(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public async Task SetBook(Book book)
        {
            await using var connection = new MySqlConnection(_settings.Value.ConnectionString);
            await connection.OpenAsync();

            var sql =
                "INSERT INTO homeworkdb.book(`AuthorFirstName`, `AuthorLastName`, `BookType`, `CountOfPages`, `ISBN`, `Price`, `Title`, `AuthorEmail`) " +
                $"VALUES('{book.AuthorFirstName}', '{book.AuthorLastName}', '{book.BookType}', '{book.CountOfPages}', " +
                $"'{book.ISBN}', {book.Price}, '{book.Title}', '{book.AuthorEmail}');";

            await connection.ExecuteAsync(sql);
        }

        public async Task SetBooks(List<Book> books)
        {
            await using var connection = new MySqlConnection(_settings.Value.ConnectionString);
            await connection.OpenAsync();

            var sqlText = books.Aggregate(new StringBuilder(), (sb, book) => sb.AppendLine(
                "INSERT INTO homeworkdb.book(`AuthorFirstName`, `AuthorLastName`, `BookType`, `CountOfPages`, `ISBN`, `Price`, `Title`, `AuthorEmail`, `CreateDate`) " +
                $"VALUES('{book.AuthorFirstName}', '{book.AuthorLastName}', '{book.BookType}', '{book.CountOfPages}', " +
                $"'{book.ISBN}', {book.Price}, '{book.Title}', '{book.AuthorEmail}', '{book.CreateDate:d}'); "));

            var sql = sqlText.ToString();
            await connection.ExecuteAsync(sql);
        }
    }
}
