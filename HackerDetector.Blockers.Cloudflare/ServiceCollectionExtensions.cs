using Microsoft.Extensions.DependencyInjection;

namespace HackerDetector.Blockers.Cloudflare
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCloudflareBlocker(this IServiceCollection services, CloudflareBlockerOptions options)
        {
            services.AddSingleton(options);
            services.AddSingleton<Functions.IHackerBlocker, CloudflareBlocker>();
        }
    }
}
