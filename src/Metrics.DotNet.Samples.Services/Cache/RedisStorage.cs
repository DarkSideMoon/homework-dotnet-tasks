using Metrics.DotNet.Samples.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Services.Cache
{
    public class RedisStorage<TItem> : IStorage<TItem> where TItem : IStorageId
    {
        private readonly IDistributedCache _redisCache;
        private readonly IRedisConnectionFactory _factory;

        public RedisStorage(IDistributedCache redisCache, IRedisConnectionFactory factory)
        {
            _redisCache = redisCache;
            _factory = factory;
        }

        /// <summary>
        /// Implementation with IDistributedCache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<TItem> Get(string key)
        {
            var stringObj = await _redisCache.GetStringAsync(BuildKey(key));
            if (!string.IsNullOrEmpty(stringObj))
            {
                return JsonConvert.DeserializeObject<TItem>(stringObj);
            }

            return default;
        }

        /// <summary>
        /// Implementation with IDistributedCache
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task Set(TItem item)
        {
            var serializedStringObj = JsonConvert.SerializeObject(item);
            var byteArray = Encoding.UTF8.GetBytes(serializedStringObj);

            await _redisCache.SetAsync(BuildKey(item.Id), byteArray);
        }

        /// <summary>
        /// Implementation with IDistributedCache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<TItem> GetOrSet(string key, TItem item)
        {
            var byteArrayObj = await _redisCache.GetAsync(key);

            if (byteArrayObj != null && byteArrayObj.Length > 0)
            {
                var stringObj = Encoding.UTF8.GetString(byteArrayObj);
                return JsonConvert.DeserializeObject<TItem>(stringObj);
            }

            var serializedStringObj = JsonConvert.SerializeObject(item);
            var byteArray = Encoding.UTF8.GetBytes(serializedStringObj);

            await _redisCache.SetAsync(BuildKey(key), byteArray);
            return item;
        }

        public async Task<IEnumerable<TItem>> GetBatch(IEnumerable<string> keys)
        {
            var redis = await _factory.ConnectAsync();
            var batch = redis.GetDatabase().CreateBatch();

            var testTasks = batch.StringGetAsync(keys.Select(x => new RedisKey(BuildKey(x))).ToArray());
            var result1 = await Task.WhenAll(testTasks);

            var getRedisBatchTasks = keys.Select(x => batch.StringGetAsync(new RedisKey(BuildKey(x))));

            batch.Execute();

            var result = await Task.WhenAll(getRedisBatchTasks);

            return result.Select(x => JsonConvert.DeserializeObject<TItem>(x));
        }

        /// <summary>
        /// Example of redis batch
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task SetBatch(IEnumerable<TItem> items)
        {
            var redis = await _factory.ConnectAsync();
            var batch = redis.GetDatabase().CreateBatch();

            var setRedisBatchTasks = items
                .Select(x => batch.StringSetAsync(
                    new RedisKey(BuildKey(x.Id)), new RedisValue(JsonConvert.SerializeObject(x)), TimeSpan.FromMinutes(5)));

            batch.Execute();

            await Task.WhenAll(setRedisBatchTasks);
        }

        /// <summary>
        /// Example of redis pipelining
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task SetPipeline(IEnumerable<TItem> items)
        {
            var redis = await _factory.ConnectAsync();
            var redisDb = redis.GetDatabase();

            var setRedisBatchTasks = items
                .Select(x => redisDb.StringSetAsync(
                    new RedisKey(BuildKey(x.Id)), new RedisValue(JsonConvert.SerializeObject(x)), TimeSpan.FromMinutes(5)));

            await Task.WhenAll(setRedisBatchTasks);
        }

        public string BuildKey(string key) => $"{typeof(TItem).Name}_{key}";

        public async Task PingPipelineAsync(int countOfPing)
        {
            var redis = await _factory.ConnectAsync();
            var redisDb = redis.GetDatabase();

            for (int i = 0; i < countOfPing; i++)
            {
                await redisDb.PingAsync();
            }
        }

        public async Task PingAsync(int countOfPing)
        {
            var redis = await _factory.ConnectAsync();
            var redisDb = redis.GetDatabase();

            var pingRedisTasks = new List<Task>();
            for (int i = 0; i < countOfPing; i++)
            {
                pingRedisTasks.Add(redisDb.PingAsync());
            }

            await Task.WhenAll(pingRedisTasks);
        }
    }
}
