using Microsoft.Extensions.Options;
using SpeechGPT.Application.Configurations;

namespace SpeechGPT.WebApi
{
    public static class ConfigurationsDependencyInjection
    {
        public static IServiceCollection AddCustomConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtProviderConfiguration>(configuration.GetSection("Jwt"));
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<JwtProviderConfiguration>>().Value);

            return services;
        }
    }
}
