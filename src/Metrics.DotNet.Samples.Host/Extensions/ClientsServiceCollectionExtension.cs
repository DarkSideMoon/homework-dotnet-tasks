using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Cache;
using Metrics.DotNet.Samples.Services.Client;
using Metrics.DotNet.Samples.Services.Repository;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Metrics.DotNet.Samples.Host.Extensions
{
    public static class ClientsServiceCollectionExtension
    {
        public static IServiceCollection AddClientsServices(this IServiceCollection services)
        {
            services.AddTransient<IElasticSearchBookClient, ElasticSearchBookClient>();
            services.AddTransient<IMongoDbBookRepository, MongoDbBookRepository>();
            services.AddTransient<IPostgresBookRepository, PostgresBookRepository>();

            services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
            services.AddSingleton<IStorage<Book>>(
                x => new RedisStorage<Book>(x.GetRequiredService<IRedisConnectionFactory>()));

            return services;
        }
    }
}
