using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.Entities.Extensions;
using SourceFuse.E_commerce.Entities.Links;

namespace SourceFuse.E_commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryBusiness _categoryBusiness;

        public CategoryController(ICategoryBusiness categoryBusiness)
        {
            _categoryBusiness = categoryBusiness;
        }
        public record UrlQueryParameters
        {
            public string Name { get; init; }
            public bool? IsActive { get; init; } = null;
            public int Limit { get; init; } = 20;
            public int Page { get; init; } = 1;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryBusiness.GetByIdAsync(id);
                if (category == null) return NoContent();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet(Name = nameof(GetCategory))]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategory([FromQuery] UrlQueryParameters urlQueryParameters, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var categories = await _categoryBusiness.GetAsync(
                    urlQueryParameters.Name,
                    urlQueryParameters.IsActive,
                    urlQueryParameters.Limit,
                    urlQueryParameters.Page,
                    cancellationToken
                );

                return Ok(GeneratePageLinks(urlQueryParameters, categories));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        private PagedModelDto GeneratePageLinks(UrlQueryParameters queryParameters, PagedModelDto response)
        {

            if (response.CurrentPage > 1)
            {
                var prevRoute = Url.RouteUrl(nameof(GetCategory), new { limit = queryParameters.Limit, page = queryParameters.Page - 1 });

                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

            }

            if (response.CurrentPage < response.TotalPages)
            {
                var nextRoute = Url.RouteUrl(nameof(GetCategory), new { limit = queryParameters.Limit, page = queryParameters.Page + 1 });

                response.AddResourceLink(LinkedResourceType.Next, nextRoute);
            }

            return response;
        }

        [Authorize(Roles = "ROLE_USER")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] CategoryRequestDTO model)
        {
            try
            {
                var category = await _categoryBusiness.AddAsync(model);
                if (category == null) return NoContent();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "ROLE_USER,ROLE_ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryUpdateRequestDTO model)
        {
            try
            {
                var categoria = await _categoryBusiness.UpdateAsync(id, model);

                if (categoria == null) return NoContent();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "ROLE_USER,ROLE_ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                if (await _categoryBusiness.DeleteAsync(id))
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("An unkown error occurred when trying to delete Category.");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
