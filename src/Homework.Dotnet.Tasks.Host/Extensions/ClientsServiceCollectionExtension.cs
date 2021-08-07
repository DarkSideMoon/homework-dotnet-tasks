using Homework.Dotnet.Tasks.Contracts;
using Homework.Dotnet.Tasks.Services.Cache;
using Homework.Dotnet.Tasks.Services.Client;
using Homework.Dotnet.Tasks.Services.Repository;
using Homework.Dotnet.Tasks.Services.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Homework.Dotnet.Tasks.Host.Extensions
{
    public static class ClientsServiceCollectionExtension
    {
        public static IServiceCollection AddClientsServices(this IServiceCollection services)
        {
            services.AddTransient<IElasticSearchBookClient, ElasticSearchBookClient>();
            services.AddTransient<IMongoDbBookRepository, MongoDbBookRepository>();
            services.AddTransient<IPostgresBookRepository, PostgresBookRepository>();
            services.AddTransient<IMySqlBookRepository, MySqlBookRepository>();

            services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
            services.AddSingleton<IStorage<Book>>(
                x => new RedisStorage<Book>(x.GetRequiredService<IRedisConnectionFactory>()));

            return services;
        }
    }
}
