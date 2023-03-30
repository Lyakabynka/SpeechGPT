using SpeechGPT.Application.Options;
using SpeechGPT.Application.Options.OptionSetups;

namespace SpeechGPT.WebApi
{
    public static class ConfigurationsDependencyInjection
    {
        public static IServiceCollection AddCustomConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(
                configuration.GetSection(JwtOptions.JwtSection));

            services.ConfigureOptions<JwtBearerOptionsSetup>();


            services.Configure<ChatGPTOptions>(
                configuration.GetSection(ChatGPTOptions.ChatGPTSection));

            return services;
        }
    }
}
