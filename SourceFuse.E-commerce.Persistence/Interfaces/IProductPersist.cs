using Microsoft.AspNetCore.Http;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Categories;
using SourceFuse.E_commerce.DTO.Responses.Products;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence.Interfaces
{
    public interface IProductPersist
    {
        Task<Tuple<int, List<Product>>> FetchPage(int page = 1, int pageSize = 5);
        Task Create(Product product);

        Task<Task> Update(Product product);

        Task<int> Delete(long id);

        Task<int> Delete(Product product);

        Task<Product> FetchById(long id, bool onlyIfPublished = false);
        Task<List<Product>> FetchAll();

        Product FetchByIdSync(long id);

        Task<Product> FetchBySlug(string slug);
        Task<Tuple<int, List<Product>>> FetchBySearchTerm(string term, int page, int pageSize);
        Task<List<Product>> FetchByIdInRetrieveNamePriceAndSlug(IEnumerable<long> productIds);
        Task<Product> GetProductBySlug(string slug);
        Task<Tuple<int, List<Product>>> FetchPageByCategory(string category, int pageSize, int page);
        Task<long> FetchBoughtManyTimes(Product product);
        Task SaveProduct(Product product);

        Task<Product> Create(string name, string description,
            IEnumerable<CategoryOnlyNameResponseDTO> categoryOnlyNameDtos);

        Task<Product> Create(CreateProductRequestDTO productDTO, bool processTags = true, bool processCategories = true);

        Task<Product> Update(string slug, CreateOrEditProductResponseDTO dto);
        Product DeleteProductById(long id);
        Task<int> Delete(string slug);
    }
}
