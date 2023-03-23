using Microsoft.Extensions.DependencyInjection;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Application.Services;

namespace SpeechGPT.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IEmailService,EmailService>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(typeof(IAppDbContext).Assembly);
            });

            //add auto mapper

            return services;
        }
    }
}