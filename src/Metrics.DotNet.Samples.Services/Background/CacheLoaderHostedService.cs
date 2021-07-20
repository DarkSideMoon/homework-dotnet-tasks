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
    public class CacheLoaderHostedService : BackgroundService
    {
        private static readonly ILogger Logger = Log.ForContext<CacheLoaderHostedService>();

        private readonly IPostgresBookRepository _postgresRepository;
        private readonly IStorage<Book> _cacheStorage;

        public CacheLoaderHostedService(IPostgresBookRepository postgresRepository, IStorage<Book> cacheStorage)
        {
            _postgresRepository = postgresRepository;
            _cacheStorage = cacheStorage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var allBooks = (await _postgresRepository.GetAllBooks()).ToList();

            for (var i = 0; i < 100; i++)
            {
                var batchBooks = allBooks.Skip(i * 1000).Take(1000);
                await _cacheStorage.SetPipelineWithFireAndForget(batchBooks);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.Information("{CacheLoaderHostedService} started", nameof(CacheLoaderHostedService));
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.Information("{CacheLoaderHostedService} stopped", nameof(CacheLoaderHostedService));
            return base.StopAsync(cancellationToken);
        }
    }
}
