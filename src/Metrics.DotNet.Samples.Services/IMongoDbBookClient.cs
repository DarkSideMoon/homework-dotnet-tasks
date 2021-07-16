using Metrics.DotNet.Samples.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Services
{
    public interface IMongoDbBookClient
    {
        Task<Book> GetBook(Guid id);

        Task<List<Book>> GetAllBooks();

        Task SetBook(Book book);

        Task SetBooks(List<Book> book);
    }
}
