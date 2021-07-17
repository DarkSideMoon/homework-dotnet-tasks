using Metrics.DotNet.Samples.Services.Client;
using Metrics.DotNet.Samples.Services.Repository;
using Metrics.DotNet.Samples.Services.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Metrics.DotNet.Samples.Host v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
