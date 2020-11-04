using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SPapi.NET;
using SpBot.Core;
using SpBot.Core.Jobs;

namespace SpBot
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("config.json");
            AppConfiguration = builder.Build();
        }

        private IConfiguration AppConfiguration;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<BotHostedService>();
            services.AddSingleton<Bot>();
            services.AddSingleton<SpClient>();
            services.AddSingleton<JobFactory>();
            services.AddSingleton<IServerInfoNotifier, ServerInfoNotifier>();
            services.AddSingleton<DataScheduler>();
            services.AddSingleton<ServerInfoNotifierJob>();

            services.Configure<BotConfiguration>(AppConfiguration);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Bot has been successfully started");
                });
            });
        }
    }
}
