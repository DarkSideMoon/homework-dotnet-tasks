using HealthChecks.UI.Client;
using Metrics.DotNet.Samples.Services.Client;
using Metrics.DotNet.Samples.Services.Repository;
using Metrics.DotNet.Samples.Services.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System;

namespace Metrics.DotNet.Samples.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
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

            services.AddOptions();
            services.Configure<ElasticSearchSetting>(Configuration.GetSection("elasticSearch"));
            services.Configure<MongoSettings>(Configuration.GetSection("mongo"));
            services.Configure<PostgresSettings>(Configuration.GetSection("postgres"));

            services.AddTransient<IElasticSearchBookClient, ElasticSearchBookClient>();
            services.AddTransient<IMongoDbBookRepository, MongoDbBookRepository>();

            services.AddHealthChecksUI(x =>
            {
                x.SetHeaderText("Metrics.DotNet.Samples.Host Health Checks Status");
                x.AddHealthCheckEndpoint("Base health check", "/healthcheck");
                x.AddHealthCheckEndpoint("Db health check", "/healthcheck/db");
            }).AddInMemoryStorage();

            services.AddHealthChecks()
                .AddCheck("base", () => HealthCheckResult.Healthy("Service is healthy!"), tags: new[] { "base" })
                .AddNpgSql(Configuration["postgres:connectionString"], tags: new string[] { "db", "sql", "postgreSql" })
                .AddMongoDb(Configuration["mongo:connectionString"], tags: new string[] { "db", "document", "mongoDb" })
                .AddElasticsearch(Configuration["elasticSearch:uri"], tags: new string[] { "db", "elasticsearch" })

                //.AddProcessHealthCheck(System.Reflection.Assembly.GetEntryAssembly().FullName, p => p.Length > 0, tags: new string[] { "base" })
                //.AddRedis(Configuration["Data:ConnectionStrings:Redis"]);
                ;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Metrics.DotNet.Samples.Host v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI(x => x.AddCustomStylesheet("style/dotnet.css"));

                endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("base"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/healthcheck/db", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("db"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
