
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Services;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Management.CloudFoundry;

using Steeltoe.Management.Endpoint.Metrics;
using Steeltoe.Management.Exporter.Metrics;
using Steeltoe.Management.Exporter.Tracing;
using Steeltoe.Management.Tracing;

namespace Orders
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add managment endpoint services
            services.AddCloudFoundryActuators(Configuration);

            // Add metrics endpoing services
            services.AddMetricsActuator(Configuration);

            // Add Cloud Foundry metrics forwarder service
            services.AddMetricsForwarderExporter(Configuration);

            // Add Distributed tracing
            services.AddDistributedTracing(Configuration);

            // Export traces to Zipkin
            services.AddZipkinExporter(Configuration);

            // Adds the configuration data POCO configured with data returned from the Spring Cloud Config Server
            services.Configure<AppConfiguration>(Configuration);

            // Add Payment Hystrix command to services
            services.AddHystrixCommand<IPaymentService, PaymentServiceCommand>("PaymentService", Configuration);

            // Add Hystrix metrics stream to enable monitoring 
            services.AddHystrixMetricsStream(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            // Add management endpoints into pipeline
            app.UseCloudFoundryActuators();

            // Add metrics collection to the app
            app.UseMetricsActuator();

            app.UseMvc();

            // Start up the metrics forwarder service added above
            app.UseMetricsExporter();

            // Start up Distributed trace exporter
            app.UseTracingExporter();

            // Startup Hystrix metrics stream
            app.UseHystrixMetricsStream();
        }
    }
}
