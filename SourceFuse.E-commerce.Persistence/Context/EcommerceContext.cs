using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Seeds;

namespace SourceFuse.E_commerce.Persistence.Context
{
    public class EcommerceContext : IdentityDbContext
        <
            ApplicationUser,
            ApplicationRole,
            long,
            IdentityUserClaim<long>,
            AppUserRole,
            IdentityUserLogin<long>,
            IdentityRoleClaim<long>,
            IdentityUserToken<long>>,
        IDesignTimeDbContextFactory<EcommerceContext>,
        IDbContext
    {
        public EcommerceContext()
        {
        }

        public EcommerceContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("DataSource=app.db", b => b.MigrationsAssembly("SourceFuse.E-commerce.API"));
        }

        public EcommerceContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<EcommerceContext> optionsBuilder =
                new DbContextOptionsBuilder<EcommerceContext>();
            optionsBuilder.UseSqlite("DataSource=app.db", b => b.MigrationsAssembly("SourceFuse.E-commerce.API"));

            var appDbContext = new EcommerceContext(optionsBuilder.Options);
            return appDbContext;
        }

        // In database store only Orders and Products, and Users(managed by Identity)
        // Cart and CartItems are stored in session, 
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<UserGender> UserGenders { get; set; }
        public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
        public virtual DbSet<ApplicationRole> Roles { get; set; }
        public virtual DbSet<AppUserRole> UserRoleMappings { get; set; }
        public virtual DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<AppUserRole>()
                .HasOne<ApplicationUser>(sc => sc.User)
                .WithMany(s => s.UserRoleMappings)
                .HasForeignKey(sc => sc.UserId);


            modelBuilder.Entity<AppUserRole>()
                .HasOne<ApplicationRole>(sc => sc.Role)
                .WithMany(s => s.UserRoleMappings)
                .HasForeignKey(sc => sc.RoleId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<Address>(o => o.Address)
                .WithOne(s=> s.User)
                .HasForeignKey<Address>(s => s.ApplicationUserId).IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<Customer>(o => o.Customer)
                .WithOne(s => s.User)
                .HasForeignKey<Customer>(s => s.ApplicationUserId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(d => d.Gender)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserGender");

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Slug).IsRequired();
                entity.HasIndex(p => p.Slug).IsUnique(true);
            });

            // modelBuilder.Entity<Product>().HasIndex(p => p.Slug).IsUnique(true);


            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Desc);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.ProductId });

                entity.Property(e => e.CategoryId);

                entity.Property(e => e.ProductId);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(o => o.User).WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId).IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(o => o.OrderId).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(o => o.Address).WithMany((string)null)
                    .HasForeignKey(o => o.AddressId).IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(oi => oi.User)
                    .WithMany((string)null).HasForeignKey(oi => oi.UserId)
                    .IsRequired(false).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId).IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public static async Task<Task> Seed(IServiceProvider services)
        {
            await Seeder.Seed(services);
            return Task.CompletedTask;
        }

        public bool EnsureDatabaseCreated()
            => Database.EnsureCreated();
    }
}
