using HealthChecks.UI.Client;
using Homework.Dotnet.Tasks.Host.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homework.Dotnet.Tasks.Host
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
            services.ConfigureCore();

            services.AddSwaggerService();

            services.ConfigureSettings(Configuration);

            services.AddClientsServices();

            services.ConfigureServiceHealthChecks(Configuration);

            services.ConfigureServiceMetrics();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Metrics.DotNet.Samples.Host v1"));

            app.UseRouting();
            app.UseAuthorization();

            app.UseMetricsAllMiddleware();
            app.UseHealthAllEndpoints();

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
