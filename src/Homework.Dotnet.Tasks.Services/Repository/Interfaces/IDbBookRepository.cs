using System.Collections.Generic;
using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Contracts;

namespace Homework.Dotnet.Tasks.Services.Repository.Interfaces
{
    public interface IDbBookRepository
    {
        Task<Book> GetBook(int id);

        Task<IEnumerable<Book>> GetAllBooks();

        Task SetBook(Book book);

        Task SetBooks(List<Book> books);
    }
}
