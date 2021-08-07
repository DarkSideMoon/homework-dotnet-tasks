using App.Metrics;
using App.Metrics.Timer;

namespace Homework.Dotnet.Tasks.Host
{
    public static class MetricsHelper
    {
        private const string DefaultContext = "HomeworkDotNetTasks";

        public static TimerOptions PostgresGetTimer = new()
        {
            Context = DefaultContext,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Milliseconds,
            Name = "timer.postgres.get"
        };

        public static TimerOptions RedisGetTimer = new()
        {
            Context = DefaultContext,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Milliseconds,
            Name = "timer.redis.get"
        };

        public static TimerOptions MongodbGetTimer = new()
        {
            Context = DefaultContext,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Milliseconds,
            Name = "timer.mongodb.get"
        };

        public static TimerOptions ElasticGetTimer = new()
        {
            Context = DefaultContext,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Milliseconds,
            Name = "timer.elastic.get"
        };
    }
}
