using Metrics.DotNet.Samples.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Services.Repository
{
    public interface IDbBookRepository
    {
        Task<Book> GetBook(Guid id);

        Task<IEnumerable<Book>> GetAllBooks();

        Task SetBook(Book book);

        Task SetBooks(List<Book> book);
    }
}
