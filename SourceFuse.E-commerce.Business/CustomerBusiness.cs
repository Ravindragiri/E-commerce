using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Customers;
using SourceFuse.E_commerce.DTO.Responses.Contact;

namespace SourceFuse.E_commerce.Business
{
    public class CustomerBusiness: ICustomerBusiness
    {
        private readonly IGenericPersist<Customer> _genericPersist;
        private readonly ICustomerPersist _customerPersist;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerBusiness(IGenericPersist<Customer> genericPersist, ICustomerPersist customerPersist, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _genericPersist = genericPersist;
            _customerPersist = customerPersist;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<CustomerResponseDTO> AddAsync(CustomerRequestDTO model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                var cliente = _mapper.Map<Customer>(model);

                _genericPersist.Add(cliente);
                if (await _genericPersist.SaveChangesAsync())
                {
                    var clienteRetorno = await _customerPersist.GetByIdAsync(cliente.Id);
                    return _mapper.Map<CustomerResponseDTO>(clienteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<CustomerResponseDTO> UpdateAsync(int customerId, CustomerRequestDTO model)
        {
            try
            {
                //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var user = await _userManager.FindByIdAsync(userId);

                var cliente = await _customerPersist.GetByIdAsync(customerId);
                if (cliente == null) return null;

                model.Id = cliente.Id;
                _mapper.Map(model, cliente);

                _genericPersist.Update(cliente);
                if (await _genericPersist.SaveChangesAsync())
                {
                    var clienteRetorno = await _customerPersist.GetByIdAsync(cliente.Id);
                    return _mapper.Map<CustomerResponseDTO>(clienteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            try
            {
                var cliente = await _customerPersist.GetByIdAsync(customerId);
                if (cliente == null) throw new Exception("Customer Not Found.");

                _genericPersist.Delete(cliente);
                return await _genericPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<CustomerResponseDTO> GetByIdAsync(int customerId)
        {
            try
            {
                var cliente = await _customerPersist.GetByIdAsync(customerId);
                if (cliente == null) return null;

                var resultado = _mapper.Map<CustomerResponseDTO>(cliente);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<PagedModelDto> GetAsync(string name, int limit, int page, CancellationToken cancellationToken)
        {
            try
            {
                var customers = await _customerPersist.SearchByName(name, limit, page, cancellationToken);
                if (customers == null) return null;

                return new AddressListResponseDTO
                {
                    CurrentPage = customers.CurrentPage,
                    TotalPages = customers.TotalPages,
                    TotalItems = customers.TotalItems,
                    Items = customers.Items.ToList()
                };

            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }
    }
}
