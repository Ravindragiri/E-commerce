using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using SourceFuse.E_commerce.API.Extensions;
using SourceFuse.E_commerce.Common.Exceptions;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceFuse.E_commerce.Persistence.Context;
using SourceFuse.E_commerce.DTO.Responses.Categories;
using SourceFuse.E_commerce.DTO.Responses.Products;
using SourceFuse.E_commerce.DTO.Requests;

namespace SourceFuse.E_commerce.Persistence
{
    public class ProductPersist : IProductPersist
    {
        private readonly EcommerceContext _context;

        public ProductPersist(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<Tuple<int, List<Product>>> FetchPage(int page = 1, int pageSize = 5)
        {
            var queryable = _context.Products;
            var count = await queryable.CountAsync();
            var results = await queryable.Skip((page - 1) * pageSize)
                .Take(pageSize) // If I do .Include(p => p.ProductImages an exception is thrown, workaround:
                                //TODO
                .Select(p => new { Product = p })
                .ToListAsync();

            return await Task.FromResult(Tuple.Create(count, results.Select(a => a.Product).ToList()));
        }

        public async Task<List<Product>> FetchAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> FetchById(long id, bool onlyIfPublished = false)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(a => a.Id == id);

            if (product != null)
            {
                return await Task.FromResult(product);
            }
            else
            {
                return await Task.FromResult<Product>(null);
            }
        }


        public Product FetchByIdSync(long id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Tuple<int, List<Product>>> FetchBySearchTerm(string term, int page, int pageSize)
        {
            var queryable = _context.Products
                .Where(a =>
                    a.Description.ToLower().Contains(term.ToLower()) ||
                    a.Name.ToLower().Contains(term.ToLower()));
            // End
            var count = queryable.Count();

            var products = await queryable.OrderByDescending(a => a.PublishAt)
                .ThenByDescending(a => a.UpdatedAt)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Include(a => a.ProductCategories)
                .ThenInclude(ac => ac.Category).ToListAsync();


            return Tuple.Create(count, products);
        }

        public async Task<Tuple<int, List<Product>>> FetchPageByCategory(string category, int pageSize, int page)
        {
            IQueryable<Product> queryable = _context.Products.Include(a => a.ProductCategories)
                .Where(a => a.ProductCategories.Any(ar =>
                    ar.Category.Name.ToLower().Contains(category.ToLower())));
            var count = queryable.Count();

            List<Product> products = await queryable.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Tuple.Create(count, products);
        }

        public async Task<Product> FetchBySlug(string slug)
        {
            return await _context.Products.FirstOrDefaultAsync(p =>
                p.Slug.ToLower().Contains(slug.ToLower()));
        }

        public async Task<long> FetchBoughtManyTimes(Product product)
        {
            return await _context.OrderItems.Where(oi => oi.Product.Id == product.Id).GroupBy(oi => oi.Order)
                .CountAsync();
        }


        public async Task<Product> GetProductBySlug(string slug)
        {
            var result = await _context.Products
                .Where(p => p.Slug.ToLower().Contains(slug.ToLower()))
                .Select(p => new { Product = p })
                .FirstAsync();

            return await Task.FromResult(result.Product);
        }

        public async Task<Product> GetByIdWithNamePriceAndSlugAsync(long productId)
        {
            return await _context.Products.Select(p => new Product
            { Id = p.Id, Name = p.Name, Slug = p.Slug, Price = p.Price })
                .FirstAsync(p => p.Id == productId);
        }

        public async Task<List<Product>> FetchByIdInRetrieveNamePriceAndSlug(IEnumerable<long> productIds)
        {
            return await _context.Products.Where(p => productIds.Contains(p.Id)).Select(p =>
                new Product { Id = p.Id, Name = p.Name, Slug = p.Slug, Price = p.Price, Stock = p.Stock }).ToListAsync();
        }


        public async Task Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }


        public async Task SaveProduct(Product product)
        {
            if (product.Id == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                Product dbEntry = _context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.ProductCategories = product.ProductCategories;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Product> Create(string name, string description,
            IEnumerable<CategoryOnlyNameResponseDTO> categoryOnlyNameDtos)
        {
            List<ProductCategory> categories = new List<ProductCategory>();

            if (categoryOnlyNameDtos == null)
                categoryOnlyNameDtos = new List<CategoryOnlyNameResponseDTO>();


            var product = new Product
            {
                Name = name,
                Description = description,
                // CreatedAt = DateTime.UtcNow,
                // UpdatedAt = DateTime.UtcNow,
                Slug = name.Slugify()
            };

            foreach (var categoryName in categoryOnlyNameDtos)
            {
                Category category =
                    await _context.Categories.Where(t => t.Name == categoryName.Name).FirstOrDefaultAsync();
                if (category == null)
                {
                    // IS there a find or create?
                    category = new Category
                    {
                        Name = categoryName.Name,
                    };
                    categories.Add(new ProductCategory
                    {
                        Category = category,
                        Product = product
                    });
                }
            }

            product.ProductCategories = categories;

            EntityEntry<Product> productEntity = await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();
            return productEntity.Entity;
        }

        public Product DeleteProductById(long id)
        {
            Product dbEntry = _context.Products.FirstOrDefault(p => p.Id == id);
            if (dbEntry != null)
            {
                _context.Products.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }


        public async Task<Product> Update(string slug, CreateOrEditProductResponseDTO dto)
        {
            var product = await _context.Products.Include(a => a.ProductCategories)
                .Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if (product == null)
            {
                throw new ResourceNotFoundException();
            }

            product.Name = dto.Name.Trim();
            product.Slug = dto.Name.Trim().Slugify();
            product.Description = dto.Description;


            if (dto.Categories != null && dto.Categories.Count() > 0)
            {
                foreach (var category in dto.Categories)
                {
                    Category cat = _context.Categories
                        .SingleOrDefault(c => c.Name.Equals(category.Name));

                    if (cat == null)
                    {
                        cat = new Category
                        {
                            Name = category.Name,
                        };

                        _context.Categories.Add(cat);
                        await _context.SaveChangesAsync();
                    }

                    // If the product was not yet associated to the category then do it now
                    if (!product.ProductCategories.Any(ac => ac.Product == product && ac.Category == cat))
                    {
                        product.ProductCategories.Add(new ProductCategory
                        {
                            Product = product,
                            Category = cat
                        });
                    }
                }
            }


            if (_context.ChangeTracker.Entries().First(x => x.Entity == product).State == EntityState.Modified)
            {
                product.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return product;
        }


        public async Task<Product> Create(CreateProductRequestDTO productDTO, bool processTags = true, bool processCategories = true)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                // CreatedAt = DateTime.UtcNow,
                // UpdatedAt = DateTime.UtcNow,
                Price = productDTO.Price,
                Stock = productDTO.Stock,
                Slug = productDTO.Name.Slugify()
            };

            ICollection<ProductCategory> productCategories = new List<ProductCategory>();

            // Find the categories on the database, if they do not exist then create them
            for (int i = 0; i < productDTO.Categories.Count; i++)
            {
                Category category;

                // If categories have been already retrieved and checked if they already existed in the database then skip to else block
                // otherwise make sure if the category exist, if it is not, then create it, if it exists then just retrieve it
                if (processCategories)
                {
                    CategoryRequestDTO categoryAt = productDTO.Categories[i];
                    category =
                        await _context.Categories.Where(t => t.Name == categoryAt.Name).FirstOrDefaultAsync();
                    if (category == null)
                    {
                        // IS there a find or create?
                        category = new Category
                        {
                            Name = categoryAt.Name,
                            Desc = categoryAt.Desc
                        };
                    }
                }
                else
                {
                    CategoryRequestDTO categoryAt = productDTO.Categories[i];
                    category = new Category
                    {
                        Name = categoryAt.Name,
                        Desc = categoryAt.Desc
                    };
                }

                productCategories.Add(new ProductCategory
                {
                    Category = category,
                    Product = product
                });
            }

            product.ProductCategories = productCategories;

            EntityEntry<Product> productEntityEntry = await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();
            return productEntityEntry.Entity;
        }


        public async Task<Task> Update(Product product)
        {
            // TODO: Is this a valid way of updating?
            // _context.Products.Update(product);
            // _context.Update(product);
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Task.CompletedTask;
        }


        public async Task<int> DeleteBySlugNotUsed(string slug)
        {
            Product product = await FetchBySlug(slug);
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(string slug)
        {
            var product = await _context.Products.Select(p => new Product { Id = p.Id })
                .FirstOrDefaultAsync(p => p.Slug == slug);
            if (product == null)
                throw new ResourceNotFoundException();

            EntityEntry<Product> entityEntry = _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(long id)
        {
            Product product = await FetchById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> Delete(Product product)
        {
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }
    }
}
