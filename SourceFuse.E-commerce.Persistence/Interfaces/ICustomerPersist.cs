using SourceFuse.E_commerce.Entities.Pagination;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence.Interfaces
{
    public interface ICustomerPersist
    {
        Task<Customer> GetByIdAsync(long customerId);
        Task<PagedModel<Customer>> SearchByName(string name, int limit, int page, CancellationToken cancellationToken);
    }
}
