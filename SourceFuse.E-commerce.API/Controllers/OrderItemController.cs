using AutoMapper;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.Entities;

namespace SourceFuse.E_commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemBusiness _orderItemBusiness;
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;

        public OrderItemController(IOrderItemBusiness orderItemBusiness,
            IMapper mapper,
            IUsersService usersService)
        {
            _orderItemBusiness = orderItemBusiness;
            _mapper = mapper;
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<OrderItem> orderItems = _orderItemBusiness.GetAll();
            return Ok(orderItems);
        }

        [HttpGet("{orderId}")]
        public IActionResult Get(int orderId)
        {
            var orderItems = _orderItemBusiness.GetById(orderId);
            if (orderItems == null)
            {
                return NotFound();
            }
            return Ok(orderItems);
        }

        [HttpPost]
        public IActionResult Post(CreateOrderItemRequestDTO orderItemDTO)
        {
            OrderItem orderItem = _mapper.Map<OrderItem>(orderItemDTO);
            long userId = Convert.ToInt64(_usersService.GetCurrentUserId());
            orderItem.UserId = userId;

            bool added = _orderItemBusiness.Add(orderItem);
            if (!added)
            {
                return BadRequest("Failed to add Order Item");
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(OrderItemUpdateRequestDTO orderItemDTO)
        {
            OrderItem orderItem = _mapper.Map<OrderItem>(orderItemDTO);
            bool updated = _orderItemBusiness.Update(orderItem);
            if (updated)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(long Id)
        {
            bool deleted = _orderItemBusiness.Delete(Id);
            if (deleted)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
