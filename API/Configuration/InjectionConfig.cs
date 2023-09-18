
using API.Infrastructure;
using API.Interfaces;
using API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configuration
{
    public static class InjectionConfig
    {
        public static IServiceCollection ResolveDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DadosBrasilApi>(configuration.GetSection("BrasilApiSettings"));

            
            services.AddSingleton<IGravaLogService, GravaLogService>();
            services.AddSingleton<IBrasilApiService, BrasilApiService>();

            return services;
        }
    }
}
