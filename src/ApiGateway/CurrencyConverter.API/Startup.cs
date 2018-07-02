using CurrencyConverter.API.Models;
using CurrencyConverter.API.Services;
using CurrencyConverter.API.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace CurrencyConverter.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Configuration.GetSection("ApplicationSettings").Bind(Singleton.AppSettings);
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();
            services.AddCors(options =>
                             {
                                 options.AddPolicy("AllowMyOrigin",
                                                   builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                             });

            services.AddScoped<IHttpClient, HttpClient>();
            services.AddScoped<ICurrencyConverterService, CurrencyConverterService>();
            services.Configure<AppSettings>(Configuration.GetSection("ApplicationSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
            app.UseCors("AllowMyOrigin");
        }
    }
}
