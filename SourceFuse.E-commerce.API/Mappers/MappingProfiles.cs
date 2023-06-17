using AutoMapper;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Categories;
using SourceFuse.E_commerce.DTO.Responses.Customers;
using SourceFuse.E_commerce.Entities;

namespace SourceFuse.E_commerce.API.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Address, AddressRequestDTO>().ReverseMap();
            
            CreateMap<Category, CategoryResponseDTO>().ReverseMap();
            CreateMap<Category, CategoryRequestDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateRequestDTO>().ReverseMap();

            CreateMap<Customer, CustomerResponseDTO>().ReverseMap();
            CreateMap<Customer, CustomerRequestDTO>().ReverseMap();

            CreateMap<OrderItem, OrderItemUpdateRequestDTO>().ReverseMap();
            CreateMap<OrderItem, CreateOrderItemRequestDTO>().ReverseMap();
        }
    }
}
