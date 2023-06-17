using SourceFuse.E_commerce.DTO;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Categories;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business.Interfaces
{
    public interface ICategoryBusiness
    {
        Task<CategoryResponseDTO> AddAsync(CategoryRequestDTO model);
        Task<CategoryResponseDTO> UpdateAsync(int categoryId, CategoryUpdateRequestDTO model);
        Task<bool> DeleteAsync(int categoryId);
        Task<CategoryResponseDTO> GetByIdAsync(int categoryId);
        Task<PagedModelDto> GetAsync(string name, bool? isActive, int limit, int page, CancellationToken cancellationToken);
    }
}
