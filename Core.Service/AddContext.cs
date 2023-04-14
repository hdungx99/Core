using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Service
{
    public static class AddContext
    {
        public static void AddContextRegist(this IServiceCollection services, IConfiguration config)
        {
            //services.AddDbContext<DbContext>(options =>
            //               options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            // Cache
            //For In-Memory Caching
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config["ConnectionString:Redis"];
                options.InstanceName = "SampleInstance";
            });

            //hang Fire
            services.AddHangfire(x => x.UseSqlServerStorage(config.GetConnectionString("SQLConfiguration")));
            services.AddHangfireServer();
        }
    }
}
