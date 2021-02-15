using AutoMapper;
using Checkers.Api.Hubs;
using Checkers.Api.Repositories;
using Checkers.Api.Settings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace Checkers.Api
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
            services.AddMediatR(typeof(Startup));
            services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:3000")
                        .WithOrigins("*")
                        .AllowCredentials();
                });
            });
            services.Configure<CheckersDbSettings>(Configuration.GetSection(nameof(CheckersDbSettings)));
            services.AddSingleton<ICheckersDbSettings>(sp => sp.GetRequiredService<IOptions<CheckersDbSettings>>().Value);
            services.AddSingleton<GamesRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSignalR(config =>
            {
                config.HandshakeTimeout = TimeSpan.FromSeconds(Configuration.GetValue<int>("SignalR:HandshakeTimeout"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("ClientPermission");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CheckersHub>("/hub");
            });
        }
    }
}
