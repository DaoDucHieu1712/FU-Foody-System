using AutoMapper;
using FFS.Application.DTOs.Order;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreaterOrder(OrderRequestDTO orderRequestDTO)
        {
            try
            {
                var order = await _orderRepository.CreateOrder(orderRequestDTO);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(List<OrderDetailDTO> items)
        {
            try
            {
                await _orderRepository.AddOrder(items);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> MyOrder(string id)
        {
            try
            {
                var list = await _orderRepository.FindAll(x => x.CustomerId == id && x.IsDelete == false).Include(x => x.Customer).Include(x => x.Shipper).ToListAsync();
                return Ok(_mapper.Map<List<OrderResponseDTO>>(list));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderWithStore(int id)
        {
            try
            {   
                List<Order> orders = new List<Order>();
                var oddts = await _orderDetailRepository.FindAll(x => x.StoreId == id)
                    .GroupBy(x => new {x.StoreId, x.OrderId})
                    .Select(x => new {orderId = x.Key.OrderId})
                    .ToListAsync();

                foreach (var item in oddts)
                {
                    var od = await _orderRepository.FindSingle(x => x.Id == item.orderId, x=> x.Customer, x => x.Shipper);
                    orders.Add(od);
                }
                return Ok(_mapper.Map<List<OrderResponseDTO>>(orders));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            try
            {
                var orderItems = await _orderDetailRepository.FindAll(x => x.OrderId == id, x => x.Food, x => x.Store).ToListAsync();
                return Ok(_mapper.Map<List<OrderDetailResponseDTO>>(orderItems));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderUnBook(Parameters parameters)
        {
            try
            {
				List<Order> check = _orderRepository.FindAll(x => x.OrderStatus == OrderStatus.Booked && x.ShipperId == parameters.ShipperId).ToList();
				if(check.Count > 0) {
					return BadRequest("Bạn đang có đơn hàng chưa hoàn thành!");
				}
				else
				{
					parameters.OrderStatus = OrderStatus.Unbooked;
					List<dynamic> orders = await _orderRepository.GetOrder(parameters);
					foreach (var item in orders)
					{
						item.detail = await _orderRepository.GetOrderDetail(item.Id);
					}
					int total = await _orderRepository.CountGetOrder(parameters);
					var res =
					 new
					 {
						 data = orders,
						 total = total
					 };
					return Ok(res);
				}
				
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{idShipper}/{idOrder}")]
        public async Task<IActionResult> ReceiveOrderUnbook(string idShipper, int idOrder)
        {
            try
            {
                var order = await _orderRepository.FindById(idOrder,null);
                if(order.ShipperId != null)
                {
                    throw new Exception("Đơn hàng đã có shipper khác nhận. Xin vui lòng thử lại!");
                }
                List<Order> check = _orderRepository.FindAll(x => x.ShipperId == idShipper && x.OrderStatus == OrderStatus.Booked, null).ToList();
                if (check.Count > 0)
                {
                    throw new Exception("Bạn đang có đơn hàng chưa hoàn thành!");
                }
                order.ShipperId = idShipper;
                order.OrderStatus = OrderStatus.Booked;

                await _orderRepository.Update(order);
                return Ok("Nhận đơn hàng thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderFinish(Parameters parameters)
        {
            try
            {
                parameters.OrderStatus = OrderStatus.Finish;
                List<dynamic> orders = await _orderRepository.GetOrder(parameters);
                foreach (var item in orders)
                {
                    item.detail = await _orderRepository.GetOrderDetail(item.Id);
                }
                int total = await _orderRepository.CountGetOrder(parameters);
                var res =
                 new
                 {
                     data = orders,
                     total = total
                 };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
