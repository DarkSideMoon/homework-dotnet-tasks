using StackExchange.Redis;
using System.Collections.Generic;

namespace Homework.Dotnet.Tasks.Services.Queue.Redis
{
    public class JobReceivedEventArgs
    {
        public JobReceivedEventArgs(Dictionary<RedisValue, RedisValue> jobs, RedisValue job)
        {
            Jobs = jobs;
            Job = job;
        }

        public Dictionary<RedisValue, RedisValue> Jobs { get; set; }

        public RedisValue Job { get; set; }
    }
}
