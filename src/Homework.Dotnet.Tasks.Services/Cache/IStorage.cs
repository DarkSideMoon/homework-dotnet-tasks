using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Contracts;

namespace Homework.Dotnet.Tasks.Services.Cache
{
    public interface IStorage<TItem> where TItem : IStorageId
    {
        Task Set(TItem item);

        Task<TItem> Get(string key);

        Task<TItem> GetOrSet(string key, Func<Task<TItem>> func);

        /// <summary>
        /// Probabilistic early expiration
        /// https://en.wikipedia.org/wiki/Cache_stampede
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="timeExpire"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        Task<TItem> GetOrSetProbabilisticItem(string key, Func<Task<TItem>> func, TimeSpan timeExpire, int beta = 1);

        Task<TItem> GetItem(string key);

        Task<TItem> GetOrSetItem(string key, Func<Task<TItem>> func);

        Task SetBatch(IEnumerable<TItem> items);

        Task SetPipeline(IEnumerable<TItem> items);

        Task SetPipelineWithFireAndForget(IEnumerable<TItem> items);

        Task<IEnumerable<TItem>> GetBatch(IEnumerable<string> keys);

        Task PingPipelineAsync(int countOfPing);

        Task PingAsync(int countOfPing);

        string BuildKey(string key);
    }
}
