using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MobileAppAPI.Config;
using MobileAppAPI.Models;
using System;

namespace MobileAppAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public LaunchSettings launchSettings { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment )
        {
            Configuration = configuration;
            launchSettings = configuration.GetSection("LaunchSettings").Get<LaunchSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
            });
            services.AddControllers();
            services.AddMvc();

            services.AddDbContext<Context>(options => {
                var connectionString = Configuration.GetConnectionString("Default");
                switch (launchSettings.Mode)
                {
                    case LaunchSettings.AppMode.Dev:
                        options.UseSqlServer(connectionString);
                        break;
                    case LaunchSettings.AppMode.Tests:
                        options.UseInMemoryDatabase("MobileAppDB");
                        break;
                    default:
                        throw new NotImplementedException();

                }
            });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}