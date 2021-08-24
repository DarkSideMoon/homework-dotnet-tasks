using Homework.Dotnet.Tasks.Contracts;
using Homework.Dotnet.Tasks.Services.Cache;
using Homework.Dotnet.Tasks.Services.Client;
using Homework.Dotnet.Tasks.Services.Repository;
using Homework.Dotnet.Tasks.Services.Repository.Interfaces;
using Homework.Dotnet.Tasks.Services.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homework.Dotnet.Tasks.Mvc.UI
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
            services.AddControllersWithViews();

            services.Configure<RedisSettings>(Configuration.GetSection("redis"));
            services.Configure<PostgresSettings>(Configuration.GetSection("postgres"));
            services.Configure<ElasticSearchSetting>(Configuration.GetSection("elasticSearch"));

            services.AddTransient<IPostgresBookRepository, PostgresBookRepository>();
            services.AddTransient<IElasticSearchBookClient, ElasticSearchBookClient>();

            services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
            services.AddSingleton<IStorage<Book>>(
                x => new RedisStorage<Book>(x.GetRequiredService<IRedisConnectionFactory>()));

            //services.AddHostedService<CacheLoaderHostedService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
