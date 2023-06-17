using SourceFuse.E_commerce.Entities.Pagination;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence.Interfaces
{
    public interface ICategoryPersist
    {
        Task<Category> GetByIdAsync(long categoryId);
        Task<PagedModel<Category>> GetAsync(string name, bool? isActive, int limit, int page, CancellationToken cancellationToken);
    }
}
