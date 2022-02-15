using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<AppDbContext>();

            return services;
        }
    }
}
