using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Services.Cache;
using StackExchange.Redis;

namespace Homework.Dotnet.Tasks.Services.Queue.Redis
{
    public class RedisQueueClient
    {
        private const string FailedCountKey = "failedcount";
        private const string ActiveKey = "active";

        public delegate RedisQueueClient Factory(string jobName, CancellationToken cancellationToken = new());

        private readonly IRedisConnectionFactory _factory;
        private readonly CancellationToken _cancellationToken;

        private readonly string _jobQueue;
        private readonly string _processingQueue;
        private readonly string _subChannel;
        private readonly string _jobName;

        private Task _managementTask;
        private bool _receiving;

        private IDatabase Database => _factory.Connect().GetDatabase();

        private string JobId => $"{_jobName}:jobid";

        public event EventHandler<JobReceivedEventArgs> OnJobReceived;

        public RedisQueueClient(string jobName, IRedisConnectionFactory factory, CancellationToken cancellationToken = new())
        {
            _jobQueue = $"{jobName}:jobs";
            _processingQueue = $"{jobName}:process";
            _subChannel = $"{jobName}:channel";
            _jobName = jobName;
            _factory = factory;
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// When a job is finished, remove it from the processingQueue and from the
        /// cache database.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="success">if false requeue for another attempt</param>
        public async Task Finish(string key, bool success = true)
        {
            var db = Database;
            await db.ListRemoveAsync(_processingQueue, key);

            if (success)
            {
                await db.KeyDeleteAsync(key);
                return;
            }

            // How many times to fail before dead
            if (await db.HashExistsAsync(key, FailedCountKey))
            {
                var count = await db.HashGetAsync(key, FailedCountKey);
                if (count.IsInteger && (int)count >= 10)
                {
                    // for now, delete the key, later we might integrate a dead message
                    // queue
                    await db.KeyDeleteAsync(key);
                    return;
                }
            }

            db.HashIncrement(key, FailedCountKey);
            db.HashDelete(key, ActiveKey);
            db.ListRightPush(_jobQueue, key);

            var connectionMultiplexer = await _factory.ConnectAsync();
            await connectionMultiplexer.GetSubscriber().PublishAsync(_subChannel, "");
        }

        // TODO: Move to BackgroundService
        /// <summary>
        /// Do we consume messages from the queue
        /// </summary>
        /// <returns></returns>
        public async Task<RedisQueueClient> AsConsumer()
        {
            var connectionMultiplexer = await _factory.ConnectAsync();
            var sub = connectionMultiplexer.GetSubscriber();

            await sub.SubscribeAsync(_subChannel, async (channel, value) => await HandleNewJobs());

            // Assume on starting that we have jobs waiting to be handled
            await HandleNewJobs();

            return this;
        }

        /// <summary>
        /// Runs a Task every 10 seconds to see if any remaining items are in
        /// processing queue
        /// </summary>
        /// <returns></returns>
        public RedisQueueClient AsManager()
        {
            _managementTask = Task.Factory.StartNew(async () =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(10000, _cancellationToken);
                        var timeToKill = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds - 10000;
                        RedisValue[] values = Database.ListRange(_processingQueue);
                        foreach (var value in from value in values let activeTime = (double)Database.HashGet((string)value, "active") where activeTime < timeToKill select value)
                        {
                            await Finish(value, false);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        Trace.WriteLine("Management Thread Finished.");
                    }

                }
            }, TaskCreationOptions.LongRunning);

            return this;
        }

        /// <summary>
        /// Move key from JobQueue to processingQueue, get key value from cache.
        /// 
        /// Also set the active field. Indicates when job was retrieved so we can monitor
        /// its time.
        /// </summary>
        /// <returns></returns>
        private async Task<Dictionary<RedisValue, RedisValue>> GetJobAsync()
        {
            var db = Database;
            var value = new Dictionary<RedisValue, RedisValue>();
            while (!_cancellationToken.IsCancellationRequested)
            {
                string key = await db.ListRightPopLeftPushAsync(_jobQueue, _processingQueue);
                // If key is null, then nothing was there to get, so no value is available
                if (string.IsNullOrEmpty(key))
                {
                    value.Clear();
                    break;
                }

                await db.HashSetAsync(key, ActiveKey, (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
                value = (await db.HashGetAllAsync(key)).ToDictionary();

                // if Count is 0, remove it and check for the next job
                if (value.Count == 0)
                {
                    await db.ListRemoveAsync(_processingQueue, key);
                    continue;
                }

                value.Add("key", key);

                break;
            }
            return value;
        }

        /// <summary>
        /// We have received an indicator that new jobs are available
        /// We process until we are out of jobs.
        /// </summary>
        private async Task HandleNewJobs()
        {
            if (_receiving) return;

            _receiving = true;

            var job = await GetJobAsync();
            // If a valid job cannot be found, it will return an empty Dictionary
            while (job.Count != 0)
            {
                // Fire the Event
                OnJobReceived?.Invoke(this, new JobReceivedEventArgs(job, job["key"]));
                // Get a new job if there is one
                job = await GetJobAsync();
            }
            _receiving = false;
        }

        /// <summary>
        /// Add a job to the Queue
        /// </summary>
        /// <param name="job"></param>
        public void AddJob(RedisValue job)
        {
            Task.Run(() => AddJobAsync(job));
        }

        /// <summary>
        /// Add a job to the Queue (async)
        /// 
        /// the single RedisValue is marked as 'payload' in the hash
        /// </summary>
        /// <param name="job">payload</param>
        public async Task AddJobAsync(RedisValue job)
        {
            if (job.IsNullOrEmpty) 
                return;

            var id = await Database.StringIncrementAsync(JobId);
            var key = BuildKey(id);
            await Database.HashSetAsync(key, "payload", job);
            await Database.ListLeftPushAsync(_jobQueue, key);

            var connectionMultiplexer = await _factory.ConnectAsync();
            await connectionMultiplexer.GetSubscriber().PublishAsync(_subChannel, "");
        }

        /// <summary>
        /// Add a job to the Queue (async)
        /// 
        /// Adds a Dictionary to the message, both values are RedisValue.
        /// 
        /// Reserved names for dictionary keys are 'key', 'active', 'failedcount'
        /// </summary>
        /// <param name="parametersDictionary"></param>
        /// <returns></returns>
        public async Task AddJobAsync(Dictionary<RedisValue, RedisValue> parametersDictionary)
        {
            if (parametersDictionary.Count == 0) return;
            if (parametersDictionary.ContainsKey("key")
                || parametersDictionary.ContainsKey(ActiveKey)
                || parametersDictionary.ContainsKey(FailedCountKey))
            {
                Trace.WriteLine("parameter 'key', 'active' or 'failedcount' are reserved.");
                return;
            }

            var db = Database;
            var id = await db.StringIncrementAsync(JobId);
            var key = BuildKey(id);

            await db.HashSetAsync(key,
                parametersDictionary.Select(entries => new HashEntry(entries.Key, entries.Value)).ToArray());

            await db.ListLeftPushAsync(_jobQueue, key);

            var connectionMultiplexer = await _factory.ConnectAsync();
            await connectionMultiplexer.GetSubscriber().PublishAsync(_subChannel, "");
        }

        private string BuildKey(long id) => $"{_jobName}:{id}";
    }
}