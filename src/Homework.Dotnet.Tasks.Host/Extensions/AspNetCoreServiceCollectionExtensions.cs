using System;
using App.Metrics;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Health;
using Homework.Dotnet.Tasks.Services.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using HealthCheckResult = Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult;

namespace Homework.Dotnet.Tasks.Host.Extensions
{
    public static class AspNetCoreServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureCore(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }

        public static IServiceCollection ConfigureServiceHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecksUI(x =>
            {
                x.SetHeaderText("Metrics.DotNet.Samples.Host Health Checks Status");
                x.AddHealthCheckEndpoint("Base health check", "/healthcheck");
                x.AddHealthCheckEndpoint("Db health check", "/healthcheck/db");
            }).AddInMemoryStorage();

            services.AddHealthChecks()
                .AddCheck("base", () => HealthCheckResult.Healthy("Service is healthy!"), tags: new[] { "base" })
                .AddNpgSql(configuration["postgres:connectionString"], tags: new[] { "db", "sql", "postgreSql" })
                .AddMongoDb(configuration["mongo:connectionString"], tags: new[] { "db", "document", "mongoDb" })
                .AddElasticsearch(configuration["elasticSearch:uri"], tags: new[] { "db", "elasticsearch" })
                .AddRedis(configuration["Data:ConnectionStrings:Redis"]);

            return services;
        }

        public static IServiceCollection ConfigureServiceMetrics(this IServiceCollection services)
        {
            var metrics = AppMetrics.CreateDefaultBuilder()
                .Configuration.Configure(x =>
                {
                    x.WithGlobalTags(
                        (tags, envInfo) =>
                        {
                            tags.Add("app_version", envInfo.EntryAssemblyVersion);
                        });
                }).Build();

            services.AddMetrics(metrics);
            services.AddMetricsTrackingMiddleware();

            // Important!!!
            services.AddMetricsReportingHostedService();
            services.AddHealthReportingHostedService();

            services.AddAppMetricsHealthPublishing();

            services.AddMetricsEndpoints(options =>
            {
                options.MetricsTextEndpointOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
            });

            var health = AppMetricsHealth.CreateDefaultBuilder()
                .HealthChecks.RegisterFromAssembly(services)
                .Report.ToMetrics(metrics)
                .BuildAndAddTo(services);

            services.AddHealth(health);

            return services;
        }

        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ElasticSearchSetting>(configuration.GetSection("elasticSearch"));
            services.Configure<MongoSettings>(configuration.GetSection("mongo"));
            services.Configure<PostgresSettings>(configuration.GetSection("postgres"));
            services.Configure<RedisSettings>(configuration.GetSection("redis"));
            services.Configure<MySqlSettings>(configuration.GetSection("mysql"));

            return services;
        }

        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Metrics.DotNet.Samples.Host",
                    Version = "v1",
                    Description = "Metrics sample project",
                    Contact = new OpenApiContact
                    {
                        Name = "DarkSideMoon",
                        Url = new Uri("https://github.com/DarkSideMoon")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT licenses",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                var fileName = System.IO.Path.GetFileName($"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}.xml");
                var path = System.IO.Path.Combine(AppContext.BaseDirectory, fileName);
                c.IncludeXmlComments(path);
            });

            return services;
        }
    }
}
