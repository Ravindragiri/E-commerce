using Microsoft.AspNetCore.Http;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Categories;
using SourceFuse.E_commerce.DTO.Responses.Products;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly IProductPersist _productPersist;
        public ProductBusiness(IProductPersist productPersist)
        {
            _productPersist = productPersist;
        }

        public async Task Create(Product product)
        {
            await _productPersist.Create(product);
        }

        public async Task<Product> Create(string name, string description, IEnumerable<CategoryOnlyNameResponseDTO> categoryOnlyNameDtos)
        {
            return await _productPersist.Create(name, description, categoryOnlyNameDtos);
        }

        public async Task<Product> Create(CreateProductRequestDTO productDTO, bool processCategories = true)
        {
            return await _productPersist.Create(productDTO, processCategories);
        }

        public async Task<int> Delete(long id)
        {
            return await _productPersist.Delete(id);
        }

        public async Task<int> Delete(Product product)
        {
            return await _productPersist.Delete(product);
        }

        public async Task<int> Delete(string slug)
        {
            return await _productPersist.Delete(slug);
        }

        public Product DeleteProductById(long id)
        {
            return _productPersist.DeleteProductById(id);
        }

        public async Task<List<Product>> FetchAll()
        {
            return await _productPersist.FetchAll();
        }

        public async Task<long> FetchBoughtManyTimes(Product product)
        {
            return await _productPersist.Delete(product);
        }

        public async Task<Product> FetchById(long id, bool onlyIfPublished = false)
        {
           return await _productPersist.FetchById(id, onlyIfPublished);
        }

        public async Task<List<Product>> FetchByIdInRetrieveNamePriceAndSlug(IEnumerable<long> productIds)
        {
            return await _productPersist.FetchByIdInRetrieveNamePriceAndSlug(productIds);
        }

        public Product FetchByIdSync(long id)
        {
            return _productPersist.FetchByIdSync(id);
        }

        public async Task<Tuple<int, List<Product>>> FetchBySearchTerm(string term, int page, int pageSize)
        {
            return await _productPersist.FetchBySearchTerm(term, page, pageSize);
        }

        public async Task<Product> FetchBySlug(string slug)
        {
            return await _productPersist.FetchBySlug(slug);
        }

        public async Task<Tuple<int, List<Product>>> FetchPage(int page = 1, int pageSize = 5)
        {
            return await _productPersist.FetchPage(page, pageSize);
        }

        public async Task<Tuple<int, List<Product>>> FetchPageByCategory(string category, int pageSize, int page)
        {
            return await _productPersist.FetchPageByCategory(category, pageSize, page);
        }

        public async Task<Product> GetProductBySlug(string slug)
        {
            return await _productPersist.GetProductBySlug(slug);  
        }

        public async Task SaveProduct(Product product)
        {
            await _productPersist.SaveProduct(product);
        }

        public async Task<Task> Update(Product product)
        {
            return await _productPersist.Update(product);
        }

        public async Task<Product> Update(string slug, CreateOrEditProductResponseDTO dto)
        {
            return await _productPersist.Update(slug, dto);
        }
    }
}
