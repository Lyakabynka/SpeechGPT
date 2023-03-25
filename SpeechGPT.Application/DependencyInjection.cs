using Microsoft.Extensions.DependencyInjection;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Application.Services;
using System.Reflection;

namespace SpeechGPT.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IEmailService,EmailService>();

            services.AddScoped<JwtProvider>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            //add auto mapper

            return services;
        }
    }
}