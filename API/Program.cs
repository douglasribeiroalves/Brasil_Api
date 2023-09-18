
using API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;
using System;

namespace API
{
    public class Program
    {

        private static string _env = "Development";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            SetEnvironment();         

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
               .Enrich.FromLogContext()
               .Enrich.WithExceptionDetails()
               .Enrich.WithCorrelationId()
               .Enrich.WithProperty("ApplicationName", $"Brasil API {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}")
               .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
               .Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("Business error"))
               .WriteTo.Async(wt => wt.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj} {NewLine}{Exception}"))
               .CreateLogger();
                      
            CreateHostBuilder(args).Build().Run();
        }

        private static void SetEnvironment()
        {
            try
            {
                var config = new ConfigurationBuilder().
                    AddJsonFile("appsettings.json", false)
                    .Build();
                _env = config.GetSection("Environment").Value;

            }
            catch (Exception)
            {
                _env = "Development";
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json");
                    config.AddJsonFile($"appsettings.{_env}.json", optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
