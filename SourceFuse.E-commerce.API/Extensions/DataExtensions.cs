using Microsoft.EntityFrameworkCore;
using SourceFuse.E_commerce.Persistence.Context;

namespace SourceFuse.E_commerce.API.Extensions
{
    public static class DataExtensions
    {
        public static void AddDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EcommerceContext>(options =>
                options.UseSqlite(configuration.GetSection("DataSources:SQLite:ConnectionString").Value));
            services.AddScoped<IDbContext>(prov => prov.GetRequiredService<EcommerceContext>());
        }
    }
}
