using AzureStorage.Interfaces;

using AzureStorage.StorageManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Supervisor.Trackers;

namespace Supervisor
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
            var AzureConnection = Configuration.GetConnectionString("AzureStorage");
            var AzureTable = new AzureTableStorage(AzureConnection);
            var AzureQueue = new AzureQueueStorage(AzureConnection, "orders");
            services.AddSingleton<ITableStorage>(AzureTable);
            services.AddSingleton<IQueueStorage>(AzureQueue);
            services.AddSingleton<ITracker, OrderTracker>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
