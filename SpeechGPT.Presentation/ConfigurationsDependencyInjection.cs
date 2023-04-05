using SpeechGPT.Application.Options;
using SpeechGPT.Application.Options.OptionSetups;

namespace SpeechGPT.WebApi
{
    public static class ConfigurationsDependencyInjection
    {
        public static IServiceCollection AddCustomConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(
                configuration.GetRequiredSection(JwtOptions.JwtSection));

            services.ConfigureOptions<JwtBearerOptionsSetup>();

            services.Configure<ChatGPTOptions>(
                configuration.GetRequiredSection(ChatGPTOptions.ChatGPTSection));

            services.Configure<RedisOptions>(
                configuration.GetRequiredSection(RedisOptions.RedisSection));
            
            return services;
        }
    }
}
