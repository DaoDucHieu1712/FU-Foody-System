using AutoMapper;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Order;
using FFS.Application.DTOs.Others;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

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
        public async Task<IActionResult> MyOrder(string id, [FromQuery] OrderFilterDTO orderFilterDTO)
        {
            try
            {
                var queryOrders = _orderRepository.FindAll(x => x.CustomerId == id, x => x.Customer, x => x.Shipper);

                if (orderFilterDTO.SortType != null)
                {
                    switch (orderFilterDTO.SortType)
                    {
                        case "date-asc":
                            queryOrders = queryOrders.OrderBy(x => x.CreatedAt);
                            break;
                        case "date-desc":
                            queryOrders = queryOrders.OrderByDescending(x => x.CreatedAt);
                            break;
                        case "price-asc":
                            queryOrders = queryOrders.OrderBy(x => x.TotalPrice);
                            break;
                        case "price-desc":
                            queryOrders = queryOrders.OrderByDescending(x => x.TotalPrice);
                            break;
                    }
                }

                if(orderFilterDTO.OrderId != null)
                {
                    queryOrders = queryOrders.Where(x => x.Id == orderFilterDTO.OrderId);
                }

                if (orderFilterDTO.Status != null)
                {
                    queryOrders = queryOrders.Where(x => x.OrderStatus == orderFilterDTO.Status);
                }

                if (orderFilterDTO.StartDate != null)
                {
                    queryOrders = queryOrders.Where(x => x.CreatedAt >= orderFilterDTO.StartDate);
                }

                if (orderFilterDTO.EndDate != null)
                {
                    queryOrders = queryOrders.Where(x => x.CreatedAt <= orderFilterDTO.EndDate);
                }

                if (orderFilterDTO.ToPrice != null)
                {
                    queryOrders = queryOrders.Where(x => x.TotalPrice >= orderFilterDTO.ToPrice);
                }

                if (orderFilterDTO.FromPrice != null)
                {
                    queryOrders = queryOrders.Where(x => x.TotalPrice <= orderFilterDTO.FromPrice);
                }

                int pageSize = Constant.Contants.PAGE_SIZE;
                List<Order> orders = PagedList<Order>.ToPagedList(queryOrders, orderFilterDTO.PageIndex ?? 1, pageSize);
                var TotalPages = (int)Math.Ceiling(orders.Count / (double)pageSize);

                return Ok(new EntityFilter<OrderResponseDTO>()
                {
                    List = _mapper.Map<List<OrderResponseDTO>>(orders),
                    PageIndex = orderFilterDTO.PageIndex ?? 1,
                    Total = TotalPages,
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderWithStore(int id, [FromQuery] OrderFilterDTO orderFilterDTO)
        {
            try
            {
                var queryOrders = _orderDetailRepository.FindAll(x => x.StoreId == id, x => x.Order)
                    .Include(x => x.Order)
                    .ThenInclude(x => x.Customer)
                    .Include(x => x.Order)
                    .ThenInclude(x => x.Shipper)
                    .GroupBy(x => new
                    {
                        Id = x.OrderId,
                        CustomerId = x.Order.CustomerId,
                        ShipperId = x.Order.ShipperId,
                        PaymentId = x.Order.PaymentId,
                        CustomerName = x.Order.Customer.UserName,
                        ShipperName = x.Order.Shipper.FirstName + " " + x.Order.Shipper.LastName,
                        CancelReason = x.Order.CancelReason,
                        Location = x.Order.Location,
                        Note = x.Order.Note,
                        PhoneNumber = x.Order.PhoneNumber,
                        TotalPrice = x.Order.TotalPrice,
                        OrderStatus = x.Order.OrderStatus,
                        CreatedAt = x.Order.CreatedAt,
                        UpdatedAt = x.Order.UpdatedAt,
                    })
                    .Select(x => new OrderResponseDTO
                    {
                        Id = x.Key.Id,
                        CustomerId = x.Key.CustomerId,
                        ShipperId = x.Key.ShipperId,
                        CustomerName = x.Key.CustomerName,
                        ShipperName = x.Key.ShipperName,
                        CancelReason = x.Key.CancelReason,
                        Location = x.Key.Location,
                        Note = x.Key.Note,
                        PhoneNumber = x.Key.PhoneNumber,
                        OrderStatus = x.Key.OrderStatus,
                        TotalPrice = x.Key.TotalPrice,
                        PaymentId = x.Key.PaymentId,
                        CreatedAt = x.Key.CreatedAt,
                    });

                if(orderFilterDTO.SortType != null)
                {
                    switch (orderFilterDTO.SortType)
                    {
                        case "date-asc":
                            queryOrders = queryOrders.OrderBy(x => x.CreatedAt);
                            break;
                        case "date-desc":
                            queryOrders = queryOrders.OrderByDescending(x => x.CreatedAt);
                            break; 
                        case "price-asc":
                            queryOrders = queryOrders.OrderBy(x => x.TotalPrice);
                            break;
                        case "price-desc":
                            queryOrders = queryOrders.OrderByDescending(x => x.TotalPrice);
                            break;
                    }
                }

                if(orderFilterDTO.CustomerName != null)
                {
                    queryOrders = queryOrders.Where(x => x.CustomerName.ToLower().Contains(orderFilterDTO.CustomerName.ToLower()));
                } 

                if(orderFilterDTO.ShipperName != null)
                {
                    queryOrders = queryOrders.Where(x => x.ShipperName.ToLower().Contains(orderFilterDTO.ShipperName.ToLower()));
                }
                
                if(orderFilterDTO.Status != null)
                {
                    queryOrders = queryOrders.Where(x => x.OrderStatus == orderFilterDTO.Status);
                }

                if (orderFilterDTO.StartDate != null)
                {
                    queryOrders = queryOrders.Where(x => x.CreatedAt >= orderFilterDTO.StartDate);
                }

                if (orderFilterDTO.EndDate != null)
                {
                    queryOrders = queryOrders.Where(x => x.CreatedAt <= orderFilterDTO.EndDate);
                }

                if(orderFilterDTO.ToPrice != null)
                {
                    queryOrders = queryOrders.Where(x => x.TotalPrice >= orderFilterDTO.ToPrice);
                }

                if (orderFilterDTO.FromPrice != null)
                {
                    queryOrders = queryOrders.Where(x => x.TotalPrice <= orderFilterDTO.FromPrice);
                }

                int pageSize = Constant.Contants.PAGE_SIZE;
                List<OrderResponseDTO> orders = PagedList<OrderResponseDTO>.ToPagedList(queryOrders, orderFilterDTO.PageIndex ?? 1, pageSize);
                var TotalPages = (int)Math.Ceiling(orders.Count / (double)pageSize);

                return Ok(new EntityFilter<OrderResponseDTO>()
                {
                    List = orders,
                    PageIndex = orderFilterDTO.PageIndex ?? 1,
                    Total = TotalPages,
                });
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

        [HttpGet]
        public async Task<IActionResult> GetOrderUnBook()
        {
            try
            {
                var orders = await _orderRepository.FindAll(x => x.OrderStatus == OrderStatus.Unbooked && x.ShipperId == null).ToListAsync();
                return Ok(_mapper.Map<List<OrderResponseDTO>>(orders));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{idShipper}/{idOrder}")]
        public async Task<IActionResult> ReceiveOrderUnbook(string idShipper, int idOrder)
        {
            try
            {
                var order = await _orderRepository.FindById(idOrder,null);
                order.ShipperId = idShipper;
                await _orderRepository.Update(order);
                return Ok("Nhận đơn hàng thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
