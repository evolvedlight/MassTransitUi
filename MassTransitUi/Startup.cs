using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransitUi.Hubs;
using MassTransitUi.Models;
using MassTransitUi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransitUi
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
            services.AddDbContext<MassTransitUiContext>(options =>
                options.UseSqlite("Data Source=masstransitui.db"));

            services.AddControllers();

            services.Configure<MassTransitSettings>(Configuration.GetSection(nameof(MassTransitSettings)));

            services.AddSpaStaticFiles(c =>
            {
                c.RootPath = "clientdist";
            });

            services.AddSignalR();

            services.AddSingleton<IErrorPipelineService, ErrorPipelineService>();

            services.AddHostedService<RabbitErrorQueuesMonitor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ErrorQueueHub>("/errorQueueHub");
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MassTransitUiContext>();
                context.Database.Migrate();
            }

            app.UseSpa(c =>
            {
                if (env.IsDevelopment() && Environment.UserInteractive && Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "false")
                {
                    c.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
