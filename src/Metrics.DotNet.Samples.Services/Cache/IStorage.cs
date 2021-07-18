using Metrics.DotNet.Samples.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Services.Cache
{
    public interface IStorage<TItem> where TItem : IStorageId
    {
        Task Set(TItem item);

        Task<TItem> Get(string key);

        Task<TItem> GetOrSet(string key, TItem item);

        Task SetBatch(IEnumerable<TItem> items);

        Task SetPipeline(IEnumerable<TItem> items);

        Task<IEnumerable<TItem>> GetBatch(IEnumerable<string> keys);

        Task PingPipelineAsync(int countOfPing);

        Task PingAsync(int countOfPing);

        string BuildKey(string key);
    }
}
