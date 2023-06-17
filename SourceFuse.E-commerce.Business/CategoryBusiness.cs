using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Categories;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business
{
    public class CategoryBusiness: ICategoryBusiness
    {
        private readonly IGenericPersist<Category> _genericPersist;
        private readonly ICategoryPersist _categoryPersist;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryBusiness(IGenericPersist<Category> genericPersist,
            ICategoryPersist categoryPersist,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _genericPersist = genericPersist;
            _categoryPersist = categoryPersist;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<CategoryResponseDTO> AddAsync(CategoryRequestDTO model)
        {
            try
            {
                //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var user = await _userManager.FindByIdAsync(userId);

                var category = _mapper.Map<Category>(model);

                _genericPersist.Add(category);
                if (await _genericPersist.SaveChangesAsync())
                {
                    var categoriaRetorno = await _categoryPersist.GetByIdAsync(category.Id);
                    return _mapper.Map<CategoryResponseDTO>(categoriaRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<CategoryResponseDTO> UpdateAsync(int categoriaId, CategoryUpdateRequestDTO model)
        {
            try
            {
                //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var user = await _userManager.FindByIdAsync(userId);

                var category = await _categoryPersist.GetByIdAsync(categoriaId);
                if (category == null) return null;

                model.Id = category.Id;
                _mapper.Map(model, category);

                _genericPersist.Update(category);
                if (await _genericPersist.SaveChangesAsync())
                {
                    var categoryResult = await _categoryPersist.GetByIdAsync(category.Id);
                    return _mapper.Map<CategoryResponseDTO>(categoryResult);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteAsync(int categoriaId)
        {
            try
            {
                //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var user = await _userManager.FindByIdAsync(userId);

                var category = await _categoryPersist.GetByIdAsync(categoriaId);
                if (category == null) throw new Exception("Categoria não encontrado.");

                _genericPersist.Delete(category);
                return await _genericPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<CategoryResponseDTO> GetByIdAsync(int categoriaId)
        {
            try
            {
                var category = await _categoryPersist.GetByIdAsync(categoriaId);
                if (category == null) return null;

                var resultado = _mapper.Map<CategoryResponseDTO>(category);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<PagedModelDto> GetAsync(string name, bool? isActive, int limit, int page, CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _categoryPersist.GetAsync(name, isActive, limit, page, cancellationToken);
                if (categories == null || categories == null) return null;

                return new CategoryListResponseDTO
                {
                    CurrentPage = categories.CurrentPage,
                    TotalPages = categories.TotalPages,
                    TotalItems = categories.TotalItems,
                    Items = categories.Items.ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }
    }
}
