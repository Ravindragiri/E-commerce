using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SourceFuse.E_commerce.API.Controllers;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.API.UnitTest.Helpers;
using SourceFuse.E_commerce.Business;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Orders;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Interfaces;
using Xunit;

namespace SourceFuse.E_commerce.API.UnitTest
{
    public class OrderControllerUnitTest
    {
        private Mock<IOrderPersist> _orderPersistMock;
        private OrderController _orderController;

        public OrderControllerUnitTest()
        {
            _orderPersistMock = MockHelper.GetPersistMock<IOrderPersist>();

            _orderController = GetOrderController();
        }

        private OrderController GetOrderController()
        {
            Mock<IUsersService> usersServiceMock = new();

            IOrderBusiness orderBusiness = new OrderBusiness(_orderPersistMock.Object);

            return new OrderController(orderBusiness, usersServiceMock.Object);
        }

        [Fact]
        public void GetAll_Test()
        {
            List<Order> orders = new List<Order>();
            var tuple = Tuple.Create(default(int), orders);
            _orderPersistMock
                .Setup(e => e.FetchPageFromUser(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(tuple);

            var httpContext = new DefaultHttpContext(); // or mock a `HttpContext`
            httpContext.Request.Headers["token"] = "fake_token_here"; //Set header
                                                                      //Controller needs a controller context 
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            _orderController.ControllerContext = controllerContext;

            int page = default(int);
            int pageSize = default(int);


            var result = _orderController.GetAll(page, pageSize).Result as ObjectResult;
            var responseDTO = result.Value as OrdersListResponseDTO;

            Assert.NotNull(responseDTO);
        }

        [Fact]
        public void GetOrdersById_Test()
        {
            Order order = new Order() { Address = new Address() };
            _orderPersistMock
                .Setup(e => e.FetchById(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(order);

            var orderId = default(int);
            var result = _orderController.GetOrdersById(orderId).Result as ObjectResult;
            var responseDTO = result.Value as OrderDetailsResponseDTO;

            Assert.NotNull(responseDTO);
        }

        [Fact]
        public void CreateOrder_Test()
        {
            Order order = new Order() { Address = new Address() };
            _orderPersistMock
                .Setup(e=> e.Create(It.IsAny<CreateOrderRequestDTO>(), It.IsAny<ApplicationUser>()))
                .ReturnsAsync(order);

            var httpContext = new DefaultHttpContext(); // or mock a `HttpContext`
            httpContext.Request.Headers["token"] = "fake_token_here"; //Set header
                                                                      //Controller needs a controller context 
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            _orderController.ControllerContext = controllerContext;

            CreateOrderRequestDTO form = default(CreateOrderRequestDTO);
            var result = _orderController.CreateOrder(form).Result as ObjectResult;
            var responseDTO = result.Value as OrderDetailsResponseDTO;

            Assert.NotNull(responseDTO);
        }
    }
}
