
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpeechGPT.Application.Interfaces;

namespace SpeechGPT.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) 
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, b=>b.MigrationsAssembly("SpeechGPT.WebApi"));

                // will not track entities if any changes occure
                // i need to specify by myself in PUT queries ( .AsTracking )
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped<IAppDbContext>(provider => 
                provider.GetRequiredService<AppDbContext>());

            return services;
        }
    }
}
