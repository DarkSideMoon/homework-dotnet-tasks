/*
using Beanstalk.Core;
using System;
using System.Threading.Tasks;

namespace Homework.Dotnet.Tasks.Services.Queue.Beanstalk
{
    public interface IBeanstalkClient
    {
        Task Publish(string message);

        Task<IJob> Consume();
    }

    public class BeanstalkClient : IBeanstalkClient
    {
        private readonly BeanstalkConnection _connection;
        private readonly string _queue;

        /// <summary>
        /// IDisposable object
        /// </summary>
        /// <example>
        /// services.AddTransient(new BeanstalkConnection("127.0.0.1", 11300));
        /// </example>
        /// <param name="connection"></param>
        /// <param name="queue"></param>
        public BeanstalkClient(BeanstalkConnection connection, string queue)
        {
            _connection = connection;
            _queue = queue;
        }

        public async Task Publish(string message)
        {
            await _connection.Use(_queue);
            await _connection.Put(message);
        }

        public async Task<IJob> Consume()
        {
            await _connection.Watch(_queue);
            return await _connection.Reserve(TimeSpan.FromSeconds(1));
        }
    }
}
*/