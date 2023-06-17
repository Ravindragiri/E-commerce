using Microsoft.EntityFrameworkCore;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence.Context
{
    public interface IDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }

        DbSet<Category> Categories { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<ApplicationUser> Users { get; set; }
        DbSet<UserGender> UserGenders { get; set; }
        DbSet<ApplicationRole> Roles { get; set; }
        DbSet<AppUserRole> UserRoleMappings { get; set; }



        bool EnsureDatabaseCreated();
    }
}
