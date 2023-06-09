﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Persistence.Services;
using SpeechGPT.Persistence.Services.Helpers;

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
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped<IAppDbContext>(provider => 
                provider.GetRequiredService<AppDbContext>());

            services.AddScoped<IChatGPT, ChatGPT>();

            services.AddScoped<JwtProvider>();
            
            services.AddSingleton<RedisConnectionHelper>();
            services.AddScoped<IRedisCache, RedisCache>();

            return services;
        }
    }
}
