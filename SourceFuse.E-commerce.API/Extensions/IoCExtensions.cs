using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.API.Infrastructure.Managers;
using SourceFuse.E_commerce.Business;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.Persistence;
using SourceFuse.E_commerce.Persistence.Interfaces;
using SourceFuse.E_commerce.Persistence.Helpers.Interfaces;
using SourceFuse.E_commerce.Persistence.Helpers;

namespace SourceFuse.E_commerce.API.Extensions
{
    public static class IoCExtensions
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ICategoryBusiness, CategoryBusiness>();
            services.AddTransient<ICustomerBusiness, CustomerBusiness>();
            services.AddTransient<IOrderBusiness, OrderBusiness>();
            services.AddTransient<IOrderItemBusiness, OrderItemBusiness>();
            services.AddTransient<IProductBusiness, ProductBusiness>();
            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<IRoleBusiness, RoleBusiness>();

            services.AddTransient<ICategoryPersist, CategoryPersist>();
            services.AddTransient<ICustomerPersist, CustomerPersist>();
            services.AddTransient<IOrderPersist, OrderPersist>();
            services.AddTransient<IOrderItemPersist, OrderItemPersist>();
            services.AddTransient<IProductPersist, ProductPersist>();
            services.AddTransient<IUserPersist, UserPersist>();
            services.AddTransient<IRolePersist, RolePersist>();

            services.AddTransient(typeof(IGenericPersist<>), typeof(GenericPersist<>));
            services.AddTransient<IUserPersist, UserPersist>();

            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
            
        }
    }
}
