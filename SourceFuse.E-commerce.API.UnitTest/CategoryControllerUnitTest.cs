using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SourceFuse.E_commerce.API.Controllers;
using SourceFuse.E_commerce.API.Mappers;
using SourceFuse.E_commerce.Business;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Categories;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Entities.Pagination;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System.Net;
using Xunit;
using static SourceFuse.E_commerce.API.Controllers.CategoryController;

namespace SourceFuse.E_commerce.API.UnitTest
{
    public class CategoryControllerUnitTest
    {
        private Mock<IGenericPersist<Category>> _genericPersistMock;
        private IMapper _mapper;
        private Mock<IHttpContextAccessor> _accessorMcok;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<ICategoryPersist> _categoryPersistMock;
        private CategoryController _categoryController;

        public CategoryControllerUnitTest()
        {
            _genericPersistMock = GetGenericPersistMock();
            _mapper = GetMapper();
            _accessorMcok = GetHttpContextAccessorMock();
            _userManagerMock = GetUserManagerMock();
            _categoryPersistMock = GetCategoryPersistMock();
            _categoryController = GetCategoryController();
        }

        private Mock<IGenericPersist<Category>> GetGenericPersistMock()
        {
            Mock<IGenericPersist<Category>> genericPersistMock = new Mock<IGenericPersist<Category>>();
            genericPersistMock
                .Setup(e => e.Add(It.IsAny<Category>()))
                .Returns(true);
            genericPersistMock
                .Setup(e => e.SaveChangesAsync())
                .ReturnsAsync(true);
            genericPersistMock
                .Setup(e => e.Delete(It.IsAny<Category>()))
                .Returns(true);

            return genericPersistMock;
        }

        private IMapper GetMapper()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            return mapper;
        }

        private Mock<IHttpContextAccessor> GetHttpContextAccessorMock()
        {
            var ipAddress = "192.168.1.123";
            var accessorMcok = new Mock<IHttpContextAccessor>();
            accessorMcok
                .Setup(a => a.HttpContext.Connection.RemoteIpAddress)
                .Returns(IPAddress.Parse(ipAddress));

            return accessorMcok;
        }

        private Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            ApplicationUser user = new ApplicationUser();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            userManagerMock
                .Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMock
                .Setup(userManager => userManager.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()));
            userManagerMock
                .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            return userManagerMock;
        }

        private CategoryController GetCategoryController()
        {
            ICategoryBusiness categoryBusiness = new CategoryBusiness(_genericPersistMock.Object,
                _categoryPersistMock.Object,
                _mapper,
                _accessorMcok.Object,
                _userManagerMock.Object);

            return new CategoryController(categoryBusiness);
        }

        private Mock<ICategoryPersist> GetCategoryPersistMock()
        {
            Mock<ICategoryPersist> categoryPersistMock = new Mock<ICategoryPersist>();

            return categoryPersistMock;
        }

        private void SetupCategoryPersistMockGetByIdAsync()
        {
            Category category = new Category();
            _categoryPersistMock
                .Setup(e => e.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(category);
        }

        [Fact]
        public void GetCategoryById_Test()
        {
            SetupCategoryPersistMockGetByIdAsync();

            int categoryById = 1;
            var result = _categoryController.GetCategoryById(categoryById).Result as OkObjectResult;

            CategoryResponseDTO categoryResponseDTO = result.Value as CategoryResponseDTO;
            Assert.NotNull(categoryResponseDTO);
        }

        [Fact]
        public void GetCategory_Test()
        {
            PagedModel<Category> categories = new();
            _categoryPersistMock
                .Setup(e => e.GetAsync(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            UrlQueryParameters urlQueryParameters = new UrlQueryParameters(); 
            CancellationToken cancellationToken = default(CancellationToken);

            var result = _categoryController.GetCategory(urlQueryParameters, cancellationToken).Result as OkObjectResult;

            var categoryResponseDTO = result.Value as PagedModelDto;
            Assert.NotNull(categoryResponseDTO);
        }

        [Fact]
        public void AddCategory_Test()
        {
            SetupCategoryPersistMockGetByIdAsync();

            CategoryRequestDTO categoryRequestDTO = new CategoryRequestDTO();
            var result = _categoryController.AddCategory(categoryRequestDTO).Result as OkObjectResult;

            var categoryResponseDTO = result.Value as CategoryResponseDTO;

            Assert.NotNull(categoryResponseDTO);
        }

        [Fact]
        public void UpdateCategory_Test()
        {
            SetupCategoryPersistMockGetByIdAsync();

            int categoryId = default(int);
            CategoryUpdateRequestDTO categoryRequestDTO = new CategoryUpdateRequestDTO();
            var result = _categoryController.UpdateCategory(categoryId, categoryRequestDTO).Result as OkObjectResult;

            var categoryResponseDTO = result.Value as CategoryResponseDTO;

            Assert.NotNull(categoryResponseDTO);
        }

        [Fact]
        public void DeleteCategory_Test()
        {
            SetupCategoryPersistMockGetByIdAsync();

            int categoryId = default(int);
            var result = _categoryController.DeleteCategory(categoryId).Result as OkResult;

            Assert.Equal(200, result.StatusCode);
        }
    }
}
