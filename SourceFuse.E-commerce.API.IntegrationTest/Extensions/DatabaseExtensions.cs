using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SourceFuse.E_commerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceFuse.E_commerce.API.Extensions;
using Microsoft.AspNetCore.Http;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.API.Infrastructure.Managers;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.Business;
using SourceFuse.E_commerce.Persistence.Interfaces;
using SourceFuse.E_commerce.Persistence;

namespace SourceFuse.E_commerce.API.IntegrationTest.Extensions
{
    public static class DatabaseExtensions
    {
        public static void AddEcommerceContextForTests(this IServiceCollection services, ServiceProvider serviceProvider)
        {
            services.AddDbContext<EcommerceContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.UseInternalServiceProvider(serviceProvider);
            });
            services.AddScoped<IDbContext>(prov => prov.GetRequiredService<EcommerceContext>());
        }
    }
}
