using Microsoft.EntityFrameworkCore;
using SourceFuse.E_commerce.Entities.Pagination;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceFuse.E_commerce.Persistence.Context;
using SourceFuse.E_commerce.Persistence.Extensions;

namespace SourceFuse.E_commerce.Persistence
{
    public class CustomerPersist: ICustomerPersist
    {
        private readonly EcommerceContext _context;
        public CustomerPersist(EcommerceContext context)
        {
            _context = context;
        }
        public async Task<Customer> GetByIdAsync(long customerId)
        {
            IQueryable<Customer> query = _context.Customers.AsNoTracking();

            query = query.OrderBy(c => c.Id)
                         .Where(c => c.Id == customerId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedModel<Customer>> SearchByName(string name, int limit, int page, CancellationToken cancellationToken)
        {
            IQueryable<Customer> query = _context.Customers.AsNoTracking();

            query = query.Where(p => p.FirstName.ToLower().Contains(name.Trim().ToLower()) ||
                                     p.LastName.ToLower().Contains(name.Trim().ToLower()));

            var pagedQuery = await query.OrderBy(p => p.FirstName)
                                        .PaginateAsync(page, limit, cancellationToken);

            return pagedQuery;
        }
    }
}
