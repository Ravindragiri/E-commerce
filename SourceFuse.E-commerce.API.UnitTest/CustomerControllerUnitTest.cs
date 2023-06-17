using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SourceFuse.E_commerce.API.Controllers;
using SourceFuse.E_commerce.API.Mappers;
using SourceFuse.E_commerce.API.UnitTest.Helpers;
using SourceFuse.E_commerce.Business;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Customers;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Entities.Pagination;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System.Net;
using Xunit;

namespace SourceFuse.E_commerce.API.UnitTest
{
    public class CustomerControllerUnitTest
    {
        private Mock<IGenericPersist<Customer>> _genericPersistMock;
        private Mock<ICustomerPersist> _customerPersistMock;
        private IMapper _mapper;
        private Mock<IHttpContextAccessor> _accessorMcok;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private CustomerController _customerController;

        public CustomerControllerUnitTest()
        {
            _genericPersistMock = MockHelper.GetGenericPersistMock<Customer>();
            _customerPersistMock = MockHelper.GetPersistMock<ICustomerPersist>();
            _mapper = MockHelper.GetMapper();
            _accessorMcok = MockHelper.GetHttpContextAccessorMock();
            _userManagerMock = MockHelper.GetUserManagerMock();

            _customerController = GetCustomerController();
        }

        private CustomerController GetCustomerController()
        {
            ICustomerBusiness customerBusiness = new CustomerBusiness(_genericPersistMock.Object,
                _customerPersistMock.Object,
                _mapper,
                _accessorMcok.Object,
                _userManagerMock.Object);

            return new CustomerController(customerBusiness);
        }

        private void SetupCustomerPersistMockGetByIdAsync()
        {
            Customer customer = new Customer();
            _customerPersistMock
                .Setup(e => e.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(customer);
        }

        [Fact]
        public void GetCustomerById_Test()
        {
            SetupCustomerPersistMockGetByIdAsync();

            int customerId = default(int);
            var result = _customerController.GetCustomerById(customerId).Result as OkObjectResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void GetCustomer_Test()
        {
            PagedModel<Customer> customers = new();
            _customerPersistMock
                .Setup(e => e.SearchByName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customers);

            CustomerController.UrlQueryParameters urlQueryParameters = new CustomerController.UrlQueryParameters();
            CancellationToken cancellationToken = default(CancellationToken);

            var result = _customerController.GetCustomer(urlQueryParameters, cancellationToken).Result as OkObjectResult;

            var pagedModelDto = result.Value as PagedModelDto;
            Assert.NotNull(pagedModelDto);

        }

        [Fact]
        public void UpdateCustomer_Test()
        {
            SetupCustomerPersistMockGetByIdAsync();

            int customerId = default(int);
            CustomerRequestDTO customerRequestDTO = new();

            var result = _customerController.UpdateCustomer(customerId, customerRequestDTO).Result as OkObjectResult;

            var customerResponseDTO = result.Value as CustomerResponseDTO;

            Assert.NotNull(customerResponseDTO);
        }

        [Fact]
        public void DeleteCustomer_Test()
        {
            SetupCustomerPersistMockGetByIdAsync();

            int customerId = default(int);
            var result = _customerController.DeleteCustomer(customerId).Result as OkResult;

            Assert.Equal(200, result.StatusCode);
        }
    }
}
