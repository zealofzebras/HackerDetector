using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerDetector
{
    public static class HackerDetectorMiddlewareExtensions
    {
        public static IApplicationBuilder UseHackerDetector(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HackerDetectorMiddleware>();
        }


        public static void AddHackerDetector(this IServiceCollection services)
        {
            services.AddHackerDetector(new HackerDetectorOptions
            {
                QueueConnectionString = "hackerdetectorqueue"
            });
        }


        public static void AddHackerDetector(this IServiceCollection services, IConfigurationSection section)
        {
            services.AddHackerDetector(new HackerDetectorOptions
            {
                QueueConnectionString = section["QueueConnection"],
            });
        }

        public static void AddHackerDetector(this IServiceCollection services, HackerDetectorOptions options)
        {

            services.AddSingleton(options);
            services.AddSingleton<HackerDetector>();
        }
    }
}
