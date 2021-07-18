using Metrics.DotNet.Samples.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Services.Repository.Interfaces
{
    public interface IPostgresBookRepository : IDbBookRepository
    {
        Task BulkSetBooks(List<Book> books);

        Task<IEnumerable<Book>> GetBooks(int count);
    }
}
