using Microsoft.EntityFrameworkCore;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Pagination;
using SourceFuse.E_commerce.Persistence.Context;
using SourceFuse.E_commerce.Persistence.Extensions;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence
{
    public class CategoryPersist: ICategoryPersist
    {
        private readonly EcommerceContext _context;
        public CategoryPersist(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(long categoryId)
        {
            IQueryable<Category> query = _context.Categories.AsNoTracking();

            query = query.Where(c => c.Id == categoryId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedModel<Category>> GetAsync(string name, bool? isActive, int limit, int page, CancellationToken cancellationToken)
        {
            IQueryable<Category> query = _context.Categories.AsNoTracking();

            query = query.OrderBy(c => c.Name);

            FilterByActive(ref query, isActive);
            SearchByName(ref query, name);
            var pagedQuery = await query.OrderBy(p => p.Name)
                                        .PaginateAsync(page, limit, cancellationToken);

            return pagedQuery;
        }

        private void SearchByName(ref IQueryable<Category> query, string name)
        {
            if (!query.Any() || string.IsNullOrWhiteSpace(name))
                return;
            query = query.Where(p => p.Name.ToLower().Contains(name.Trim().ToLower()));
        }

        private void FilterByActive(ref IQueryable<Category> query, bool? isActive)
        {
            if (!query.Any() || !isActive.HasValue)
                return;
            query = query.Where(p => p.IsActive == isActive);
        }
    }
}
