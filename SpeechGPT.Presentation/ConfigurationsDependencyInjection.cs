using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using SpeechGPT.Application.Options;
using SpeechGPT.Application.Options.OptionSetups;

namespace SpeechGPT.WebApi
{
    public static class ConfigurationsDependencyInjection
    {
        public static IServiceCollection AddCustomConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.ConfigureOptions<JwtBearerOptionsSetup>();


            return services;
        }
    }
}
