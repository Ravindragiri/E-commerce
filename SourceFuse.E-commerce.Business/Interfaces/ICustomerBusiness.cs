using SourceFuse.E_commerce.DTO;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business.Interfaces
{
    public interface ICustomerBusiness
    {
        Task<CustomerResponseDTO> AddAsync(CustomerRequestDTO model);
        Task<CustomerResponseDTO> UpdateAsync(int id, CustomerRequestDTO model);
        Task<bool> DeleteAsync(int customerId);

        Task<CustomerResponseDTO> GetByIdAsync(int customerId);
        Task<PagedModelDto> GetAsync(string name, int limit, int page, CancellationToken cancellationToken);
    }
}
