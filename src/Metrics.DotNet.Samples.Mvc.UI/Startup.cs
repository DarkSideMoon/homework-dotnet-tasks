using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Background;
using Metrics.DotNet.Samples.Services.Cache;
using Metrics.DotNet.Samples.Services.Repository;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Metrics.DotNet.Samples.Services.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Metrics.DotNet.Samples.Mvc.UI
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
            
            services.AddTransient<IPostgresBookRepository, PostgresBookRepository>();

            services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
            services.AddSingleton<IStorage<Book>>(
                x => new RedisStorage<Book>(x.GetRequiredService<IRedisConnectionFactory>()));

            //services.AddHostedService<CacheLoaderHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
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
