using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using SourceFuse.E_commerce.API.Extensions;
using SourceFuse.E_commerce.API.Infrastructure.Managers;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.Common.Exceptions;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace SourceFuse.E_commerce.Persistence.Seeds
{
    public class Seeder
    {
        private static async Task SeedAdminUserAndRole(IServiceProvider services)
        {
            using (var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var scopedServices = serviceScope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<EcommerceContext>();

                IConfigurationService configurationService = services.GetService<IConfigurationService>();
                //IConfigurationService configurationService = serviceScope.ServiceProvider.GetService<ConfigurationService>();
                UserManager<ApplicationUser> userManager =
                    serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                RoleManager<ApplicationRole> roleManager =
                    serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

                string adminUserName = configurationService.GetAdminUserName();
                string adminFirstName = configurationService.GetAdminFirstName();
                string adminLastName = configurationService.GetAdminLastName();
                string adminEmail = configurationService.GetAdminEmail();
                string adminPassword = configurationService.GetAdminPassword();
                string adminRoleName = configurationService.GetAdminRoleName();

                {
                    IdentityResult authRoleCreated = IdentityResult.Success;
                    if (await roleManager.FindByNameAsync(adminRoleName) == null)
                    {
                        authRoleCreated = await roleManager.CreateAsync(new ApplicationRole(adminRoleName));
                    }

                    if (await userManager.FindByNameAsync(adminUserName) == null && authRoleCreated.Succeeded)
                    {
                        var gender = await SeedGenderAndGetGenderId(dbContext);

                        ApplicationUser user = new ApplicationUser
                        {
                            FirstName = adminFirstName,
                            LastName = adminLastName,
                            UserName = adminUserName,
                            Email = adminEmail,
                            GenderId = gender.Id,
                            //Password = adminPassword,
                        };

                        IdentityResult result = await userManager.CreateAsync(user, adminPassword);

                        if (result.Succeeded)
                        {
                            result = await userManager.AddToRoleAsync(user, adminRoleName);
                            if (!result.Succeeded)
                                throw new ThreadStateException();
                        }
                        else
                        {
                            throw new UnexpectedApplicationStateException("Failed to Create User");
                        }
                    }
                }
            }
        }

        private static async Task<UserGender> SeedGenderAndGetGenderId(EcommerceContext dbContext)
        {
            var userGender = default(UserGender);
            var firstGender = await dbContext.UserGenders.FirstOrDefaultAsync();
            if (firstGender != default(UserGender))
            {
                userGender = firstGender;
            }
            else
            {
                userGender = new UserGender()
                {
                    Gender = "Male"
                };

                await dbContext.UserGenders.AddAsync(userGender);
                await dbContext.SaveChangesAsync();
            }

            return userGender;
        }

        private static async Task SeedAuthenticatedUsersAndRole(IServiceProvider services)
        {
            Faker faker = new Faker();
            using (var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var scopedServices = serviceScope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<EcommerceContext>();

                IConfigurationService settingsService = services.GetService<IConfigurationService>();

                UserManager<ApplicationUser> userManager =
                    serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                RoleManager<ApplicationRole> roleManager =
                    serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

                string standardUserRoleName = settingsService.GetStandardUserRoleName();
                //                if (await roleManager.FindByNameAsync(roleAuthor) == null)
                IdentityResult result = IdentityResult.Success;
                if (!(await roleManager.RoleExistsAsync(standardUserRoleName)))
                {
                    result = await roleManager.CreateAsync(new ApplicationRole(standardUserRoleName));
                    if (!result.Succeeded)
                    {
                        throw new UnexpectedApplicationStateException();
                    }
                }

                if (result.Succeeded)
                {
                    EcommerceContext applicationDbContext =
                        serviceScope.ServiceProvider.GetRequiredService<EcommerceContext>();
                    var usersCount = applicationDbContext.Users.Count();
                    var usersToSeed = 43;
                    usersToSeed -= usersCount;

                    ApplicationRole r = await roleManager.FindByNameAsync(standardUserRoleName);

                    var gender = await SeedGenderAndGetGenderId(dbContext);

                    for (int i = 0; i < usersToSeed; i++)
                    {
                        ApplicationUser user = new ApplicationUser
                        {
                            FirstName = faker.Name.FirstName(),
                            LastName = faker.Name.LastName(),
                            UserName = faker.Internet.UserName(faker.Name.FirstName(), faker.Name.LastName()),
                            Email = faker.Internet.Email(),
                            Gender = gender,
                            //Password = "password"
                        };
                        result = await userManager.CreateAsync(user, "password");
                        if (result.Succeeded)
                        {
                            ApplicationUser userFindResult = await userManager.FindByNameAsync(user.UserName);
                            if (!await userManager.IsInRoleAsync(userFindResult, standardUserRoleName))
                            {
                                await userManager.AddToRoleAsync(userFindResult, standardUserRoleName);
                            }
                        }
                    }
                }
            }
        }

        private static async Task SeedCategories(IServiceProvider services)
        {
            using (var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //EcommerceContext dbContext = services.GetRequiredService<EcommerceContext>();
                var scopedServices = serviceScope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<EcommerceContext>();
                var categoryCount = await dbContext.Categories.CountAsync();
                var categoriesToSeed = 5;
                categoriesToSeed -= categoryCount;
                if (categoriesToSeed <= 0)
                    return;
                var faker = new Faker<Category>()
                    .RuleFor(t => t.Name, f => f.Lorem.Word())
                    .RuleFor(t => t.Desc, f => f.Lorem.Sentences(2));


                List<Category> categories = faker.Generate(categoriesToSeed);

                dbContext.Categories.AddRange(categories);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedProducts(IServiceProvider services)
        {
            using (var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                IConfigurationService settingsService =
                    serviceScope.ServiceProvider.GetService<IConfigurationService>();
                EcommerceContext dbContext = services.GetRequiredService<EcommerceContext>();
                var productsCount = await dbContext.Products.CountAsync();
                var productsToSeed = 35;
                productsToSeed -= productsCount;
                if (productsToSeed <= 0)
                    return;

                UserManager<ApplicationUser> userManager =
                    serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                RoleManager<ApplicationRole> roleManager =
                    serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

                var faker = new Faker<Product>()
                    .RuleFor(a => a.PublishAt, f => f.Date
                        .Between(DateTime.Now.AddYears(-3), DateTime.Now.AddYears(1)))
                    .RuleFor(a => a.Name, f => f.Commerce.ProductName()) //  f.Lorem.Sentence()
                    .RuleFor(a => a.Description, f => f.Lorem.Sentences(2))
                    .RuleFor(p => p.Price,
                        f => f.Random.Int(min: 50,
                            max: 1000)) // f.Commerce.Price(min: 50, max: 1000) will return a string
                    .RuleFor(p => p.Stock, f => f.Random.Int(min: 0, max: 2500))
                    .FinishWith(async (f, aproductInstance) =>
                    {
                        ICollection<ProductCategory> productCategories = new List<ProductCategory>();
                        productCategories.Add(new ProductCategory
                        {
                            Product = aproductInstance,
                            ProductId = aproductInstance.Id,
                            Category = await dbContext.Categories.OrderBy(t => t.Id).FirstAsync()
                        }
                        );
                        aproductInstance.ProductCategories = productCategories;

                        aproductInstance.Slug = aproductInstance.Name.Slugify();
                    });


                List<Product> products = faker.Generate(productsToSeed);
                products.ForEach(a =>
                {
                    dbContext.Products.Add(a);
                    //       dbContext.Entry(a).State = EntityState.Added;
                });
                EntityEntry<Product> entry = dbContext.Products.Add(products[0]);
                dbContext.Products.AddRange(products);
                // dbContext.ChangeTracker.DetectChanges();
                // var res = dbContext.SaveChanges();
                await dbContext.SaveChangesAsync();
            }
        }


        public static async Task SeedOrders(IServiceProvider services)
        {
            using (var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                IConfigurationService settingsService =
                    serviceScope.ServiceProvider.GetService<IConfigurationService>();
                EcommerceContext dbContext = services.GetRequiredService<EcommerceContext>();

                Random random = new Random();


                var faker = new Faker<Order>()
                    .RuleFor(o => o.TrackingNumber, f => f.Random.AlphaNumeric(16))
                    .FinishWith(async (fk, order) =>
                    {
                        // User
                        var orderingUser =
                            fk.Random.Bool(
                                0.75f) // 75% change we create an order with an authenticated user, 25% change of guest user making the order
                                ? await dbContext.Users.Include(u => u.Address).OrderBy(a => a.Id)
                                    .FirstAsync()
                                : null;
                        order.User = orderingUser;


                        // Address
                        if (orderingUser?.Address != null)
                        {
                            order.Address = orderingUser.Address;
                        }
                        else
                        {
                            order.Address = new Address
                            {
                                // we may have a user but with 0 Addresses
                                ApplicationUserId = orderingUser?.Id,
                                FirstName = fk.Name.FirstName(),
                                LastName = fk.Name.LastName(),
                                StreetAddress = fk.Address.StreetAddress(),
                                City = fk.Address.City(),
                                Country = fk.Address.Country(),
                                ZipCode = fk.Address.ZipCode()
                            };
                        }


                        // Seed OrderItems
                        var product = dbContext.Products.OrderBy(p => p.Id).First();

                        // OrderItems
                        ICollection<OrderItem> orderItems = new List<OrderItem>();
                        for (int i = 0; i < fk.Random.Int(min: 1, max: 20); i++)
                        {
                            orderItems.Add(new OrderItem
                            {
                                User = orderingUser,
                                Order = order,
                                // OrderId = order.Id,
                                Slug = product.Slug,
                                Name = product.Name,
                                Product = await dbContext.Products.OrderBy(p => p.Id).FirstAsync(),
                                Price = Math.Max(10, fk.Random.Int(min: -20, max: 20) + product.Price),
                                Quantity = fk.Random.Int(min: 1, max: 10)
                            });
                        }

                        order.OrderItems = orderItems;
                        //dbContext.Attach(c.User);
                    });

                var ordersCount = await dbContext.Orders.CountAsync();
                var ordersToSeed = 35;
                ordersToSeed -= ordersCount;

                if (ordersToSeed > 0)
                {
                    List<Order> orders = faker.Generate(ordersToSeed);
                    dbContext.Orders.AddRange(orders);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public static async Task Seed(IServiceProvider services)
        {
            await SeedAdminUserAndRole(services);
            await SeedAuthenticatedUsersAndRole(services);
            await SeedCategories(services);
            await SeedProducts(services);
            await SeedOrders(services);
        }
    }
}
