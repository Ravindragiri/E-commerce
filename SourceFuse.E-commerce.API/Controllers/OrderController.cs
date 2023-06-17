using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.BaseResponses;
using SourceFuse.E_commerce.DTO.Responses.Orders;
using SourceFuse.E_commerce.DTO.Responses.Wrappers;
using SourceFuse.E_commerce.Entities;

namespace SourceFuse.E_commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBusiness _orderBusiness;
        private readonly IUsersService _usersService;


        public OrderController(IOrderBusiness orderBusiness, IUsersService usersService)
        {
            _orderBusiness = orderBusiness;
            _usersService = usersService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            // Get Orders from current User
            long userId = Convert.ToInt64(_usersService.GetCurrentUserId());
            Tuple<int, List<Order>> orders = await _orderBusiness.FetchPageFromUser(userId);

            return StatusCodeResponseDTOWrapper.BuildSuccess(OrdersListResponseDTO.Build(orders.Item2, Request.Path,
                currentPage: page, pageSize: pageSize, totalItemCount: orders.Item1));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdersById(long id)
        {
            var order = await _orderBusiness.FetchById(id, includeOrderItems: true, includeAddress: true);
            if (order == null)
                return StatusCodeResponseDTOWrapper.BuildGeneric(new ErrorResponseDTO("Not Found"), statusCode: 404);

            //return NotFound();

            return new StatusCodeResponseDTOWrapper(OrderDetailsResponseDTO.Build(order, false));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDTO form)
        {
            var order = await _orderBusiness.Create(form, await _usersService.GetCurrentUserAsync());
            if (order != null)
            {
                return StatusCodeResponseDTOWrapper.BuildGeneric(OrderDetailsResponseDTO.Build(order));
            }
            else
            {
                return StatusCodeResponseDTOWrapper.BuildErrorResponse("Something went wrong");
            }
        }
    }
}
