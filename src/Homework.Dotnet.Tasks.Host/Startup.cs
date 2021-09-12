/*
 * Difference between logstash and filebeat 
 * https://stackoverflow.com/questions/58585855/difference-between-using-filebeat-and-logstash-to-push-log-file-to-elasticsearch
 * https://www.educba.com/filebeat-vs-logstash/
 * https://logz.io/blog/filebeat-vs-logstash/
 */

/*
 * Other resources
 * https://logz.io/blog/logstash-tutorial/
 * https://logz.io/blog/filebeat-vs-logstash/
 */

using HealthChecks.UI.Client;
using Homework.Dotnet.Tasks.Host.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Homework.Dotnet.Tasks.Host
{
    public class Startup
    {
        private static readonly ILogger _logger = Log.ForContext<Startup>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _logger.Information("Application is starting...");

            services.ConfigureCore();

            services.AddSwaggerService();

            services.ConfigureSettings(Configuration);

            services.AddClientsServices();

            services.ConfigureServiceHealthChecks(Configuration);

            services.ConfigureServiceMetrics();

            _logger.Information("Application is started...");
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
