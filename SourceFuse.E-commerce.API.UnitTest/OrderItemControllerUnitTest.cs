using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SourceFuse.E_commerce.API.Controllers;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.API.UnitTest.Helpers;
using SourceFuse.E_commerce.Business;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Persistence;
using SourceFuse.E_commerce.Persistence.Interfaces;
using Xunit;

namespace SourceFuse.E_commerce.API.UnitTest
{
    public class OrderItemControllerUnitTest
    {
        private Mock<IGenericPersist<OrderItem>> _genericPersistMock;
        private Mock<IOrderItemPersist> _orderItemPersistMock;
        private OrderItemController _orderItemController;
        private IMapper _mapper;

        public OrderItemControllerUnitTest()
        {
            _genericPersistMock = MockHelper.GetGenericPersistMock<OrderItem>();
            _orderItemPersistMock = MockHelper.GetPersistMock<IOrderItemPersist>();
            _mapper = MockHelper.GetMapper();

            _orderItemController = GetOrderItemController();
        }

        private OrderItemController GetOrderItemController()
        {
            var orderItemBusiness = new OrderItemBusiness(_genericPersistMock.Object,
                _orderItemPersistMock.Object);
            Mock<IUsersService> usersServiceMock = new();

            var orderItemController = new OrderItemController(orderItemBusiness,
                _mapper,
                usersServiceMock.Object);

            return orderItemController;
        }


        [Fact]
        public void Get_Test()
        {
            List<OrderItem> expectedOrderItems = new();
            _genericPersistMock
                .Setup(e => e.GetAll())
                .Returns(expectedOrderItems);

            var result = _orderItemController.Get() as OkObjectResult;
            var orderItems = result.Value as IEnumerable<OrderItem>;

            Assert.NotNull(orderItems);
        }

        [Fact]
        public void GetByOrderId_Test()
        {
            OrderItem expectedOrderItem = new();

            _orderItemPersistMock
                .Setup(e => e.FetchById(It.IsAny<long>()))
                .ReturnsAsync(expectedOrderItem);

            var orderId = default(int);
            var result = _orderItemController.Get(orderId) as OkObjectResult;
            var orderItem = result.Value as OrderItem;

            Assert.NotNull(orderItem);
        }

        [Fact]
        public void Post_Test()
        {
            CreateOrderItemRequestDTO orderItem = new();
            var result = _orderItemController.Post(orderItem) as OkResult;


            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Put_Test()
        {
            _orderItemPersistMock
                .Setup(e => e.Update(It.IsAny<OrderItem>()))
                .ReturnsAsync(true);

            OrderItemUpdateRequestDTO orderItem = default(OrderItemUpdateRequestDTO);
            var result = _orderItemController.Put(orderItem) as OkResult;


            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Delete_Test()
        {
            _genericPersistMock
                .Setup(e => e.DeleteById(It.IsAny<int>()))
                .Returns(true);

            var orderItemId = default(int);
            var result = _orderItemController.Delete(orderItemId) as OkResult;

            Assert.Equal(200, result.StatusCode);
        }
    }
}
