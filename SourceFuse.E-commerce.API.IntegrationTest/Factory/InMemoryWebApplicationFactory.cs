using Microsoft.AspNetCore.Hosting;
using SourceFuse.E_commerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SourceFuse.E_commerce.API.IntegrationTest.Extensions;
using SourceFuse.E_commerce.Persistence.Seeds;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Http;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.API.Infrastructure.Managers;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.Business;
using SourceFuse.E_commerce.Persistence.Interfaces;
using SourceFuse.E_commerce.Persistence;
using SourceFuse.E_commerce.Persistence.Helpers.Interfaces;
using SourceFuse.E_commerce.Persistence.Helpers;

namespace SourceFuse.E_commerce.API.IntegrationTest.Factory
{
    public class InMemoryWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                //services.AddTransient<ICategoryBusiness, CategoryBusiness>();
                //services.AddTransient<ICustomerBusiness, CustomerBusiness>();
                //services.AddTransient<IOrderBusiness, OrderBusiness>();
                //services.AddTransient<IOrderItemBusiness, OrderItemBusiness>();
                //services.AddTransient<IProductBusiness, ProductBusiness>();
                //services.AddTransient<IUserBusiness, UserBusiness>();

                //services.AddTransient<ICategoryPersist, CategoryPersist>();
                //services.AddTransient<ICustomerPersist, CustomerPersist>();
                //services.AddTransient<IOrderPersist, OrderPersist>();
                //services.AddTransient<IProductPersist, ProductPersist>();
                //services.AddTransient<IUserPersist, UserPersist>();

                //services.AddTransient(typeof(IGenericPersist<>), typeof(GenericPersist<>));
                //services.AddTransient<IUserPersist, UserPersist>();

                //services.AddScoped<IConfigurationService, ConfigurationService>();
                //services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

                //services.AddScoped<IDbContext>(prov => prov.GetRequiredService<EcommerceContext>());

                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddEcommerceContextForTests(serviceProvider);

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    //var dbContext = scopedServices.GetRequiredService<IDbContext>();
                    var dbContext = scopedServices.GetRequiredService<EcommerceContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<InMemoryWebApplicationFactory<TStartup>>>();

                    dbContext.EnsureDatabaseCreated();

                    //try
                    //{
                    //    TestDataSeeder.SeedToDoItems(scopedServices, dbContext).Wait();
                    //}
                    //catch (Exception ex)
                    //{
                    //    logger.LogError(ex, $"An error occurred seeding the database with test messages. Error: {ex.Message}");
                    //}
                }
            });
        }
    }
}
