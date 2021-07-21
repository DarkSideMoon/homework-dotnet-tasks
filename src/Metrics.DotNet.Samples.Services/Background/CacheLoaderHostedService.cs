using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Cache;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Metrics.DotNet.Samples.Services.Background
{
    public class CacheLoaderHostedService : IHostedService, IDisposable
    {
        private static readonly ILogger Logger = Log.ForContext<CacheLoaderHostedService>();

        private readonly IPostgresBookRepository _postgresRepository;
        private readonly IStorage<Book> _cacheStorage;
        private Timer _timer;

        public CacheLoaderHostedService(IPostgresBookRepository postgresRepository, IStorage<Book> cacheStorage)
        {
            _postgresRepository = postgresRepository;
            _cacheStorage = cacheStorage;
        }

        private void LoadCache(object state)
        {
            Task.Run(async () =>
            {
                var allBooks = (await _postgresRepository.GetAllBooks()).ToList();

                for (var i = 0; i < 100; i++)
                {
                    var batchBooks = allBooks.Skip(i * 1000).Take(1000);
                    await _cacheStorage.SetPipelineWithFireAndForget(batchBooks);
                }
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(LoadCache, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            Logger.Information("{CacheLoaderHostedService} started", nameof(CacheLoaderHostedService));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            Logger.Information("{CacheLoaderHostedService} stopped", nameof(CacheLoaderHostedService));
            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }
}
