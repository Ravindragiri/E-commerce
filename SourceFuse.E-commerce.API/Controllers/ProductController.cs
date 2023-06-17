using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.BaseResponses;
using SourceFuse.E_commerce.DTO.Responses.Products;
using SourceFuse.E_commerce.DTO.Responses.Wrappers;
using SourceFuse.E_commerce.Entities;
using System.Text.RegularExpressions;

namespace SourceFuse.E_commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public int PageSize = 4;

        private readonly IProductBusiness _productBusiness;
        private readonly IConfigurationService _configurationService;

        private readonly IUsersService _usersService;

        public ProductController(IConfigurationService settingsService,
            IProductBusiness productService,
            IConfigurationService configurationService, 
            IUsersService usersService)
        {
            _productBusiness = productService;
            _configurationService = configurationService;
            _usersService = usersService;
        }


        [HttpGet("")]
        [ActionName(nameof(Index))]
        public async Task<IActionResult> Index(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var products = await _productBusiness.FetchPage(page, pageSize);
            var basePath = Request.Path;

            return StatusCodeResponseDTOWrapper.BuildSuccess(ProductListResponseDTO.Build(products.Item2, basePath,
                currentPage: page, pageSize: pageSize, totalItemCount: products.Item1));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateProductRequestDTO productDTO)
        {
            if (!(await _usersService.IsAdmin()))
                return StatusCodeResponseDTOWrapper.BuildUnauthorized("Only admin user can create prodcuts");

            Product product = await _productBusiness.Create(productDTO);
            return StatusCodeResponseDTOWrapper.BuildSuccess(ProductDetailsResponseDTO.Build(product));
        }

        [HttpPut("{slug}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(string slug, [FromBody] CreateOrEditProductResponseDTO dto)
        {
            if (!ModelState.IsValid)
                return StatusCodeResponseDTOWrapper.BuilBadRequest(ModelState);


            Product product = await _productBusiness.Update(slug, dto);

            return StatusCodeResponseDTOWrapper.BuildSuccess(ProductDetailsResponseDTO.Build(product), "Updated successfully");
        }


        [HttpGet("search/{term}")]
        public async Task<IActionResult> GetBySearchTerm(string term, int page = 1, int pageSize = 5)
        {
            Tuple<int, List<Product>> result;
            if (!string.IsNullOrEmpty(term))
            {
                result = await _productBusiness.FetchBySearchTerm(term, page, pageSize);
            }
            else
                result = await _productBusiness.FetchPage(page, pageSize);

            return StatusCodeResponseDTOWrapper.BuildSuccess(ProductListResponseDTO.Build(result.Item2, "search/", page,
                pageSize, result.Item1));
        }

        [HttpGet]
        [Route("/by_category/{category}")]
        public async Task<IActionResult> GetByCategory([FromRoute] string category, int page = 1, int pageSize = 5)
        {
            Tuple<int, List<Product>> products = await _productBusiness.FetchPageByCategory(category, pageSize, page);

            return StatusCodeResponseDTOWrapper.BuildSuccess(ProductListResponseDTO.Build(products.Item2,
                "/by_category/{category}", page, pageSize, products.Item1));
        }

        [HttpGet("/by_slug/{slug}")]
        public async Task<IActionResult> GetProductBySlug(string slug)
        {
            var product = await _productBusiness.GetProductBySlug(slug);
            if (product == null)
                return StatusCodeResponseDTOWrapper.BuildNotFound(new ErrorResponseDTO("Not Found"));

            return new StatusCodeResponseDTOWrapper(ProductDetailsResponseDTO.Build(product));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            var product = await _productBusiness.FetchById(id);
            if (product == null)
                return StatusCodeResponseDTOWrapper.BuildNotFound(new ErrorResponseDTO("Not Found"));

            return StatusCodeResponseDTOWrapper.BuildGeneric(ProductDetailsResponseDTO.Build(product));
        }


        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            Product product = default;
            if (id != null)
                product = await _productBusiness.FetchById(id.Value);

            if (product == null)
            {
                return StatusCodeResponseDTOWrapper.BuildGenericNotFound();
            }

            if ((await _productBusiness.Delete(product)) > 0)
            {
                return StatusCodeResponseDTOWrapper.BuildSuccess("Product deleted successfully");
            }
            else
            {
                return StatusCodeResponseDTOWrapper.BuildErrorResponse("An error occured, try later");
            }
        }
    }
}
