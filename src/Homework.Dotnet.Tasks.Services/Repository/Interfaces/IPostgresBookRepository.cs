using System.Collections.Generic;
using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Contracts;

namespace Homework.Dotnet.Tasks.Services.Repository.Interfaces
{
    public interface IPostgresBookRepository : IDbBookRepository
    {
        Task BulkSetBooks(List<Book> books);

        Task<IEnumerable<Book>> GetBooks(int count);

        Task<Book> GetRandomBook();
    }
}
