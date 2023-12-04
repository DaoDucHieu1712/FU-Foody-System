using System.Net;
using System.Security.Cryptography;
using System.Text;

using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;

using FFS.Application.DTOs.Admin;

using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Order;
using FFS.Application.DTOs.Others;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Hubs;
using FFS.Application.Migrations;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
		private readonly IStoreRepository _storeRepository;
        private readonly IMapper _mapper;
		private readonly IHubContext<NotificationHub> _hubContext;
		private readonly INotificationRepository _notifyRepository;
		public OrderController(IOrderRepository orderRepository, IHubContext<NotificationHub> hubContext, IOrderDetailRepository orderDetailRepository, INotificationRepository notifyRepository, IStoreRepository storeRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
			_storeRepository = storeRepository;
			_hubContext = hubContext;
			_notifyRepository = notifyRepository;
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
		public async Task<IActionResult> Order(CreateOrderDTO createOrderDTO)
		{
			try
			{
				return Ok(await _orderRepository.Order(createOrderDTO));
			}
			catch (Exception ex) 
}
}
		[HttpGet("{orderId}")]
		public async Task<IActionResult> GetStoreIdByOrderId(int orderId)
		{
			try
			{
				var storeId = await _orderRepository.GetStoreIdByOrderId(orderId);
				if (storeId.HasValue)
				{
					return Ok(new { StoreId = storeId.Value });
				}
				else
				{
					return NotFound($"Store not found for OrderId: {orderId}");
				}
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
		public async Task<IActionResult> FindById(int id)
		{
			try
			{
				return Ok(_mapper.Map<OrderResponseDTO>(
					await _orderRepository.FindAll(x => x.Id == id, x => x.Customer, x => x.Shipper, x => x.Payment)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Store)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Food)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Combo)
					.FirstOrDefaultAsync()
					));
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
                var queryOrders = _orderRepository.FindAll(x => x.CustomerId == id, x => x.Customer, x => x.Shipper, x => x.Payment);

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
                var TotalPages = (int)Math.Ceiling(queryOrders.Count() / (double)pageSize);

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
					.Include(x => x.Order)
					.ThenInclude(x => x.Payment)
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
						PaymentMethod = x.Order!.Payment!.PaymentMethod,
						PaymentStatus = x.Order!.Payment!.Status,
                        CreatedAt = x.Order.CreatedAt,
                        UpdatedAt = x.Order.UpdatedAt,
						ShipDate = x.Order.ShipDate,
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
						PaymentMethod = x.Key!.PaymentMethod,
						PaymentStatus = x.Key!.PaymentStatus,
                        CreatedAt = x.Key.CreatedAt,
						ShipDate = x.Key.ShipDate,
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
                var TotalPages = (int)Math.Ceiling(queryOrders.Count() / (double)pageSize);

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
                var orderItems = await _orderDetailRepository.FindAll(x => x.OrderId == id, x => x.Food, x => x.Store, x => x.Combo).ToListAsync();
                return Ok(_mapper.Map<List<OrderDetailResponseDTO>>(orderItems));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderUnBook(DTOs.QueryParametter.Parameters parameters)
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

		[HttpGet("{id}")]
		public async Task<IActionResult> CheckReceiverOrder(string id)
		{
			try
			{
				List<Order> check = _orderRepository.FindAll(x => x.OrderStatus == OrderStatus.Booked && x.ShipperId == id).ToList();
				if (check.Count > 0)
				{
					return Ok(false);
				}
				
				return Ok(true);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetOrderIdel(int? PageIndex, int? PageSize)
		{
			try
			{
				var productsQuery = _orderRepository
					.FindAll(x => x.OrderStatus == OrderStatus.Unbooked ,x => x.Customer, x => x.Payment)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Store)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Food)
					.OrderByDescending(x => x.CreatedAt);

				PagedList<Order> orderPaged = PagedList<Order>.ToPagedList(productsQuery, PageIndex ?? 1, PageSize ?? 7);

				return Ok(new
				{
					List = _mapper.Map<List<OrderResponseDTO>>(orderPaged),
					PageIndex = orderPaged.CurrentPage,
					PageSize = orderPaged.PageSize,
					TotalPages = orderPaged.TotalPages,
					Total = orderPaged.TotalCount,
					HasPrevious = orderPaged.HasPrevious,
					HasNext = orderPaged.HasNext,
				});
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
				if (order.ShipperId != null)
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

				

				var storeId = await _orderRepository.GetStoreIdByOrderId(order.Id);


				if (!storeId.HasValue)
				{
					return NotFound($"Store not found for OrderId: {order.Id}");
				}

				var storeinfor = await _storeRepository.GetInformationStore(storeId.Value);
				//var notification = new Notification
				//{
				//	CreatedAt = DateTime.Now,
				//	UpdatedAt = DateTime.Now,
				//	IsDelete = false,
				//	UserId = storeinfor.UserId,
				//	Title = "Cập nhật đơn hàng",
				//	Content = $"Mã đơn hàng: #{order.Id} đã được shipper nhận giao."
				//};
				//await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
				//await _notifyRepository.Add(notification);
				var customerNotification = new Notification
				{
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					IsDelete = false,
					UserId = order.CustomerId,
					Title = "Cập nhật đơn hàng",
					Content = $"Đơn hàng của bạn đã được chấp nhận và đang trên đường giao. Mã đơn hàng: #{order.Id}"
				};

				await _hubContext.Clients.All.SendAsync("ReceiveNotification", customerNotification);
				await _notifyRepository.Add(customerNotification);

				
				return Ok("Nhận đơn hàng thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

		[HttpPut("{id}")]
		public async Task<IActionResult> AcceptOrderWithShipper(int id)
		{
			try
			{
				var order = await _orderRepository.FindSingle(x => x.Id == id);
				order.OrderStatus = OrderStatus.Finish;
				order.ShipDate= DateTime.Now;
				await _orderRepository.Update(order);
				var notification = new Notification
				{
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					IsDelete = false,
					UserId = order.CustomerId,
					Title = "Cập nhật đơn hàng",
					Content = $"Đơn hàng của bạn #{order.Id} đã giao thành công"
				};

				await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
				await _notifyRepository.Add(notification);

				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> CancelOrderWithCustomer(int id, string CancelReason)
		{
			try
			{
				var order = await _orderRepository.FindSingle(x => x.Id == id);
				order.OrderStatus = OrderStatus.Cancel;
				order.CancelReason = CancelReason;
				await _orderRepository.Update(order);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

        [HttpPost]
        public async Task<IActionResult> GetOrderFinish(DTOs.QueryParametter.Parameters parameters)
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

		[HttpGet("{orderId}")]
		public async Task<string> GetUrlPayment(int orderId)
		{
			Order order = await _orderRepository.FindById(orderId, null);

			List<OrderDetail> orderDetails = await _orderDetailRepository.FindAll(x => x.OrderId == orderId, null).ToListAsync();
			int storeId = orderDetails[0].StoreId;
			Store store = await _storeRepository.FindById(storeId, null);


			string vnpTmnCode = "U0K46IOK";
			string vnpHashSecret = "LKBKXTYIOEAOZAMOQJCQNDPKNKQFRKBQ";
			string vnpUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
			string vnpTxnRef = order.Id.ToString();
			string vnpOrderInfo = $"Thanh toán đơn hàng cho cửa hàng ${store.StoreName} tại ${store.Address} ";
			string vnpOrderType = "100000";	
			long vnpAmount = Convert.ToInt64(order.TotalPrice) * 100;
			string vnpLocal = "vn";
			string vnpIpAdd = HttpContext.Connection.RemoteIpAddress.ToString();

			Dictionary<string, string> inputData = new Dictionary<string, string>
				{
					{ "vnp_Amount", vnpAmount.ToString() },
					{ "vnp_Command", "pay" },
					{ "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
					{ "vnp_CurrCode", "VND" },
					{ "vnp_IpAddr", vnpIpAdd },
					{ "vnp_Locale", vnpLocal },
					{ "vnp_OrderInfo", vnpOrderInfo },
					{ "vnp_OrderType", vnpOrderType },
					{ "vnp_ReturnUrl", "http://localhost:5173/confirm-payment" },
					{ "vnp_TmnCode", vnpTmnCode },
					{ "vnp_TxnRef", vnpTxnRef },
					{ "vnp_Version", "2.1.0" },
					{ "vnp_BankCode", "NCB" },

					
				};
			inputData = SortDictionary(inputData);
			string originalData = string.Join("&", inputData.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));
			string vnpUrlWithQuery = vnpUrl + "?" + string.Join("&", inputData.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));
			string vnpSecurityHash = CalculateVnPSecurityHash(vnpHashSecret, originalData);

			vnpUrlWithQuery += $"&vnp_SecureHashType=HMACSHA512&vnp_SecureHash={vnpSecurityHash}";
			return vnpUrlWithQuery;
		}

		public class PaymentCreate
		{
			public string PaymentMethod { get; set; }
			public int OrderId { get; set; }
			public int? Status { get; set; }
		}

		[HttpPost]
		public async Task<IActionResult> CreatePayment(PaymentCreate payment)
		{
			Order order = await _orderRepository.FindById(payment.OrderId, null);

			Payment p = new Payment()
			{
				Amount = order.TotalPrice,
				PaymentMethod = payment.PaymentMethod
			};
			if (payment.Status == 2)
			{
				p.Status = PaymentStatus.Completed;
			}
			else
			{
				p.Status = PaymentStatus.Pending;
			}

			await _orderRepository.CreatePayment(p);

			order.PaymentId = p.Id;
			await _orderRepository.Update(order);


			var storeId = await _orderRepository.GetStoreIdByOrderId(order.Id);


			if (!storeId.HasValue)
			{
				return NotFound($"Store not found for OrderId: {order.Id}");
			}

			var storeinfor = await _storeRepository.GetInformationStore(storeId.Value);

			// Ensure storeinfor is valid before proceeding
			if (storeinfor == null)
			{
				return NotFound($"Store information not found for StoreId: {storeId}");
			}

			var notification = new Notification
			{
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				IsDelete = false,
				UserId = storeinfor.UserId,
				Title = "Đơn hàng mới",
				Content = $"Bạn có đơn hàng mới mã #{order.Id}"
			};

			await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
			await _notifyRepository.Add(notification);


			return Ok(p);
		}

		public class Confirm
		{
			public int OrderId { get; set; }
			public int Response { get; set; }
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmPayment(Confirm confirm)
		{
			await _orderRepository.ConfirmPayment(confirm);
			
			return Ok("Thanh toán thành công");
		}

		private string CalculateVnPSecurityHash(string vnpHashSecret, string originalData)
		{
			var secretKeyBytes = Encoding.UTF8.GetBytes(vnpHashSecret);
			using (var hmac = new HMACSHA512(secretKeyBytes))
			{
				var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(originalData));
				var signed = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
				return signed;
			}
		}

		private Dictionary<string, string> SortDictionary(Dictionary<string, string> dictionary)
		{
			return new Dictionary<string, string>(dictionary.OrderBy(kvp => kvp.Key));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderPendingWithShipper(string id)
		{
			try
			{
				var orders = await _orderRepository
					.FindAll(x => x.ShipperId == id, x => x.Customer, x => x.Shipper , x => x.Payment)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Food)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Store)
					.Where(x => x.OrderStatus == OrderStatus.Booked)
					.ToListAsync();
				var order = orders.FirstOrDefault();
				return Ok(_mapper.Map<OrderResponseDTO>(order));
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

	}
}
