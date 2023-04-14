using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Core.Service
{
    public static class AddBuilder
    {
        public static void AddBuilderRegis(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire");
        }
    }
}
