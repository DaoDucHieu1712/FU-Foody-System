using System.Net;
using System.Security.Cryptography;
using System.Text;

using AutoMapper;

using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Order;
using FFS.Application.DTOs.Others;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Hubs;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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
		private readonly IHubContext<OrderIdelHub> _orderHubContext;
		private readonly INotificationRepository _notifyRepository;
		private readonly IInventoryRepository _inventoryRepository;
		private ILoggerManager _logger;

		public OrderController(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IStoreRepository storeRepository, IMapper mapper, IHubContext<NotificationHub> hubContext, IHubContext<OrderIdelHub> orderHubContext, INotificationRepository notifyRepository, IInventoryRepository inventoryRepository, ILoggerManager logger)
		{
			_orderRepository = orderRepository;
			_orderDetailRepository = orderDetailRepository;
			_storeRepository = storeRepository;
			_mapper = mapper;
			_hubContext = hubContext;
			_orderHubContext = orderHubContext;
			_notifyRepository = notifyRepository;
			_inventoryRepository = inventoryRepository;
			_logger = logger;
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CreaterOrder(OrderRequestDTO orderRequestDTO)
		{
			try
			{
				_logger.LogInfo("Creating order");
				var order = await _orderRepository.CreateOrder(orderRequestDTO);

				_logger.LogInfo("Order created successfully");
				return Ok(order);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while creating order: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Order(CreateOrderDTO createOrderDTO)
		{
			try
			{
				_logger.LogInfo("Placing an order");
				var order = await _orderRepository.Order(createOrderDTO);

				foreach (var item in createOrderDTO.OrderDetails)
				{
					if (item.FoodId != null)
					{
						var inventory = await _inventoryRepository.FindSingle(x => x.FoodId == item.FoodId);
						inventory.quantity = inventory.quantity - item.Quantity;
						await _inventoryRepository.Update(inventory, "CreatedAt");
					}
				}
				_logger.LogInfo("Order placed successfully");

				return Ok(order);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while placing an order: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{orderId}")]
		public async Task<IActionResult> GetStoreIdByOrderId(int orderId)
		{
			try
			{
				_logger.LogInfo($"Getting store ID for Order ID: {orderId}");
				var storeId = await _orderRepository.GetStoreIdByOrderId(orderId);
				if (storeId.HasValue)
				{
					_logger.LogInfo($"Store ID retrieved successfully for Order ID: {orderId}");
					return Ok(new { StoreId = storeId.Value });
				}
				else
				{
					_logger.LogInfo($"Store not found for Order ID: {orderId}");
					return NotFound($"Store not found for OrderId: {orderId}");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting store ID: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddOrderItem(List<OrderDetailDTO> items)
		{
			try
			{
				_logger.LogInfo("Adding order items");
				await _orderRepository.AddOrder(items);

				_logger.LogInfo("Order items added successfully");
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while adding order items: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> FindById(int id)
		{
			try
			{

				var _od = await _orderRepository.FindAll(x => x.Id == id, x => x.Customer, x => x.Shipper, x => x.Payment)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Store)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Food)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Combo)
					.FirstOrDefaultAsync();
				if (_od == null) return NotFound();
				var _order = _mapper.Map<OrderResponseDTO>(_od);
				_logger.LogInfo($"Finding order by ID: {id}");
				return Ok(_order);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while finding order by ID: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> MyOrder(string id, [FromQuery] OrderFilterDTO orderFilterDTO)
		{
			try
			{
				_logger.LogInfo($"Retrieving orders for customer ID: {id}");
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
						default:
							queryOrders = queryOrders.OrderByDescending(x => x.CreatedAt);
							break;
					}
				}

				if (orderFilterDTO.OrderId != null)
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
				_logger.LogInfo($"Orders retrieved successfully for customer ID: {id}");
				return Ok(new EntityFilter<OrderResponseDTO>()
				{
					List = _mapper.Map<List<OrderResponseDTO>>(orders),
					PageIndex = orderFilterDTO.PageIndex ?? 1,
					Total = TotalPages,
				});

			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving orders for customer ID: {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderWithStore(int id, [FromQuery] OrderFilterDTO orderFilterDTO)
		{
			try
			{
				_logger.LogInfo($"Retrieving orders for store ID: {id}");
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
						ShipFee = x.Order.ShipFee
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
						ShipFee = x.Key.ShipFee
					});

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
						default:
							queryOrders = queryOrders.OrderByDescending(x => x.CreatedAt);
							break;
					}
				}

				if (orderFilterDTO.CustomerName != null)
				{
					queryOrders = queryOrders.Where(x => x.CustomerName.ToLower().Contains(orderFilterDTO.CustomerName.ToLower()));
				}

				if (orderFilterDTO.ShipperName != null)
				{
					queryOrders = queryOrders.Where(x => x.ShipperName.ToLower().Contains(orderFilterDTO.ShipperName.ToLower()));
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
				List<OrderResponseDTO> orders = PagedList<OrderResponseDTO>.ToPagedList(queryOrders, orderFilterDTO.PageIndex ?? 1, pageSize);
				var TotalPages = (int)Math.Ceiling(queryOrders.Count() / (double)pageSize);
				_logger.LogInfo($"Orders retrieved successfully for store ID: {id}");
				return Ok(new EntityFilter<OrderResponseDTO>()
				{
					List = orders,
					PageIndex = orderFilterDTO.PageIndex ?? 1,
					Total = TotalPages,
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving orders for store ID: {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderDetail(int id)
		{
			try
			{
				var order = await _orderRepository.FindSingle(x => x.Id == id);
				if (order == null) return NotFound();
				_logger.LogInfo($"Attempting to get order details for order ID {id}...");
				var orderItems = await _orderDetailRepository.FindAll(x => x.OrderId == id, x => x.Food, x => x.Store, x => x.Combo).ToListAsync();
				_logger.LogInfo($"Successfully retrieved order details for order ID {id}.");
				return Ok(_mapper.Map<List<OrderDetailResponseDTO>>(orderItems));
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting order details for order ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles ="Shipper")]
		[HttpPost]
		public async Task<IActionResult> GetOrderUnBook(DTOs.QueryParametter.Parameters parameters)
		{
			try
			{
				_logger.LogInfo($"Attempting to get unbooked orders for Shipper ID {parameters.ShipperId}...");
				List<Order> check = _orderRepository.FindAll(x => x.OrderStatus == OrderStatus.Booked && x.ShipperId == parameters.ShipperId).ToList();
				if (check.Count > 0)
				{
					_logger.LogWarn($"Shipper ID {parameters.ShipperId} has an uncompleted order. Unable to get unbooked orders.");
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
					_logger.LogInfo($"Successfully retrieved {total} unbooked orders for Shipper ID {parameters.ShipperId}.");
					return Ok(res);
				}

			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting unbooked orders for Shipper ID {parameters.ShipperId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "Shipper")]
		[HttpGet("{id}")]
		public async Task<IActionResult> CheckReceiverOrder(string id)
		{
			try
			{
				_logger.LogInfo($"Checking if receiver with ID {id} has a booked order...");
				List<Order> check = _orderRepository.FindAll(x => x.OrderStatus == OrderStatus.Booked && x.ShipperId == id).ToList();
				if (check.Count > 0)
				{
					_logger.LogInfo($"Receiver with ID {id} has a booked order.");
					return Ok(false);
				}
				_logger.LogInfo($"Receiver with ID {id} has a booked order.");
				return Ok(true);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while checking if receiver with ID {id} has a booked order: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "Shipper")]
		[HttpGet]
		public async Task<IActionResult> GetOrderIdel(int? PageIndex, int? PageSize)
		{
			try
			{
				_logger.LogInfo($"Attempting to get unbooked orders with pagination. PageIndex: {PageIndex}, PageSize: {PageSize}");
				var productsQuery = _orderRepository
					.FindAll(x => x.OrderStatus == OrderStatus.Unbooked, x => x.Customer, x => x.Payment)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Store)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Food)
					.OrderByDescending(x => x.CreatedAt);

				PagedList<Order> orderPaged = PagedList<Order>.ToPagedList(productsQuery, PageIndex ?? 1, PageSize ?? 7);
				_logger.LogInfo($"Successfully retrieved {orderPaged.Count} unbooked orders with pagination. PageIndex: {PageIndex}, PageSize: {PageSize}");

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
				_logger.LogError($"An error occurred while getting unbooked orders with pagination. PageIndex: {PageIndex}, PageSize: {PageSize}. Error: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "Shipper")]
		[HttpPut("{idShipper}/{idOrder}")]
		public async Task<IActionResult> ReceiveOrderUnbook(string idShipper, int idOrder)
		{
			try
			{
				_logger.LogInfo($"Shipper with ID {idShipper} is attempting to receive unbooked order with ID {idOrder}...");
				var order = await _orderRepository.FindById(idOrder, null);
				if (order.ShipperId != null)
				{
					_logger.LogError($"Order with ID {idOrder} already has another shipper. Unable to receive.");
					throw new Exception("Đơn hàng đã có shipper khác nhận. Xin vui lòng thử lại!");
				}
				List<Order> check = _orderRepository.FindAll(x => x.ShipperId == idShipper && x.OrderStatus == OrderStatus.Booked, null).ToList();
				if (check.Count > 0)
				{
					_logger.LogError($"Shipper with ID {idShipper} has an uncompleted order. Unable to receive.");
					throw new Exception("Bạn đang có đơn hàng chưa hoàn thành!");
				}
				order.ShipperId = idShipper;
				order.OrderStatus = OrderStatus.Booked;


				await _orderRepository.Update(order);
					


				var storeId = await _orderRepository.GetStoreIdByOrderId(order.Id);


				if (!storeId.HasValue)
				{
					_logger.LogError($"Store not found for OrderId: {order.Id}");
					return NotFound($"Store not found for OrderId: {order.Id}");
				}

				var storeinfor = await _storeRepository.GetInformationStore(storeId.Value);
				var notification = new Notification
				{
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					IsDelete = false,
					UserId = storeinfor.UserId,
					Title = "Cập nhật đơn hàng",
					Content = $"Mã đơn hàng #{order.Id} đã được shipper nhận giao."
				};

				await _hubContext.Clients.Group(storeinfor.UserId).SendAsync("ReceiveNotification", notification);

				await _notifyRepository.Add(notification);

				var customerNotification = new Notification
				{
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					IsDelete = false,
					UserId = order.CustomerId,
					Title = "Cập nhật đơn hàng",
					Content = $"Đơn hàng của bạn đã được chấp nhận và đang trên đường giao. Mã đơn hàng #{order.Id}"
				};

				await _hubContext.Clients.Group(order.CustomerId).SendAsync("ReceiveNotification", customerNotification);
				await _notifyRepository.Add(customerNotification);

				_logger.LogInfo($"Successfully received unbooked order with ID {idOrder} by Shipper ID {idShipper}.");
				return Ok("Nhận đơn hàng thành công!");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while receiving unbooked order with ID {idOrder} by Shipper ID {idShipper}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "Shipper")]
		[HttpPut("{id}")]
		public async Task<IActionResult> AcceptOrderWithShipper(int id)
		{
			try
			{
				var order = await _orderRepository.FindSingle(x => x.Id == id);
				order.OrderStatus = OrderStatus.Finish;
				order.ShipDate = DateTime.Now;
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

				//await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
				await _hubContext.Clients.Group(order.CustomerId).SendAsync("ReceiveNotification", notification);
				await _notifyRepository.Add(notification);


				var storeId = await _orderRepository.GetStoreIdByOrderId(order.Id);


				if (!storeId.HasValue)
				{
					_logger.LogError($"Store information not found for orderId: {storeId}");
					return NotFound($"Store not found for OrderId: {order.Id}");
				}

				var storeinfor = await _storeRepository.GetInformationStore(storeId.Value);

				var storeNotification = new Notification
				{
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					IsDelete = false,
					UserId = storeinfor.UserId,
					Title = "Cập nhật đơn hàng",
					Content = $"Mã đơn hàng #{order.Id} đã được shipper giao thành công."
				};

				await _hubContext.Clients.Group(storeinfor.UserId).SendAsync("ReceiveNotification", notification);

				await _notifyRepository.Add(storeNotification);

				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpPut("{id}")]
		public async Task<IActionResult> CancelOrderWithCustomer(int id, string CancelReason)
		{
			try
			{
				_logger.LogInfo($"Attempting to accept order with ID {id} and mark it as finished...");
				var order = await _orderRepository.FindSingle(x => x.Id == id);
				order.OrderStatus = OrderStatus.Cancel;
				order.CancelReason = CancelReason;
				await _orderRepository.Update(order);


				var ods = await _orderDetailRepository.FindAll(x => x.OrderId == id).ToListAsync();

				foreach (var item in ods)
				{
					var inventory = await _inventoryRepository.FindSingle(x => x.FoodId == item.FoodId);
					inventory.quantity = inventory.quantity + item.Quantity;
					await _inventoryRepository.Update(inventory, "CreatedAt");
				}

				_logger.LogInfo($"Successfully accepted order with ID {id} and marked it as finished.");
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while accepting order with ID {id} and marking it as finished: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "Shipper")]
		[HttpPut("{id}")]
		public async Task<IActionResult> CancelOrderWithShipper(int id, string CancelReason)
		{
			try
			{
				_logger.LogInfo($"Attempting to cancel order with ID {id} and provide cancel reason...");
				var order = await _orderRepository.FindSingle(x => x.Id == id);
				order.OrderStatus = OrderStatus.Cancel;
				order.CancelReason = CancelReason;
				await _orderRepository.Update(order);

				var ods = await _orderDetailRepository.FindAll(x => x.OrderId == id).ToListAsync();

				foreach (var item in ods)
				{
					var inventory = await _inventoryRepository.FindSingle(x => x.FoodId == item.FoodId);
					inventory.quantity = inventory.quantity + item.Quantity;
					await _inventoryRepository.Update(inventory, "CreatedAt");
				}
				_logger.LogInfo($"Successfully canceled order with ID {id} and provided cancel reason.");
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while canceling order with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> GetOrderFinish(DTOs.QueryParametter.Parameters parameters)
		{
			try
			{
				_logger.LogInfo($"Attempting to get finished orders with parameters: {parameters}...");
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
				_logger.LogInfo($"Successfully retrieved {total} finished orders.");
				return Ok(res);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting finished orders: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{orderId}")]
		public async Task<string> GetUrlPayment(int orderId)
		{
			try
			{
				_logger.LogInfo($"Attempting to generate payment URL for order with ID {orderId}...");
				Order order = await _orderRepository.FindById(orderId, null);

				List<OrderDetail> orderDetails = await _orderDetailRepository.FindAll(x => x.OrderId == orderId, null).ToListAsync();
				int storeId = orderDetails[0].StoreId;
				Store store = await _storeRepository.FindById(storeId, null);


				string vnpTmnCode = "U0K46IOK";
				string vnpHashSecret = "LKBKXTYIOEAOZAMOQJCQNDPKNKQFRKBQ";
				string vnpUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
				string vnpTxnRef = order.Id.ToString();
				string vnpOrderInfo = $"Thanh toán đơn hàng cho cửa hàng ${store.StoreName} tại  ";
				//${store.Address}
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
				_logger.LogInfo($"Successfully generated payment URL for order with ID {orderId}.");
				return vnpUrlWithQuery;
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while generating payment URL for order with ID {orderId}: {ex.Message}");
				throw; // Consider rethrowing the exception or handle it as needed
			}
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
			try
			{
				_logger.LogInfo($"Attempting to create payment for order with ID {payment.OrderId}...");

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
					_logger.LogError($"Store information not found for orderId: {storeId}");
					return NotFound($"Store not found for OrderId: {order.Id}");
				}

				var storeinfor = await _storeRepository.GetInformationStore(storeId.Value);

				// Ensure storeinfor is valid before proceeding
				if (storeinfor == null)
				{
					_logger.LogError($"Store information not found for StoreId: {storeId}");
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

				//await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
				await _hubContext.Clients.Group(storeinfor.UserId).SendAsync("ReceiveNotification", notification);
				await _notifyRepository.Add(notification);

				_logger.LogInfo($"Successfully created payment for order with ID {payment.OrderId}.");
				return Ok(p);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while creating payment for order with ID {payment.OrderId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		public class Confirm
		{
			public int OrderId { get; set; }
			public int Response { get; set; }
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmPayment(Confirm confirm)
		{
			try
			{
				_logger.LogInfo($"Attempting to confirm payment with ID {confirm.OrderId}...");

				await _orderRepository.ConfirmPayment(confirm);

				_logger.LogInfo($"Successfully confirmed payment with ID {confirm.OrderId}.");

				return Ok("Thanh toán thành công");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while confirming payment with ID {confirm.OrderId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
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

		[Authorize(Roles = "Shipper")]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderPendingWithShipper(string id)
		{
			try
			{
				_logger.LogInfo($"Attempting to get pending orders for shipper with ID {id}...");
				var orders = await _orderRepository
					.FindAll(x => x.ShipperId == id, x => x.Customer, x => x.Shipper, x => x.Payment)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Food)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Store)
					.Include(x => x.OrderDetails).ThenInclude(x => x.Combo)
					.Where(x => x.OrderStatus == OrderStatus.Booked)
					.ToListAsync();
				var order = orders.FirstOrDefault();
				_logger.LogInfo($"Successfully retrieved pending orders for shipper with ID {id}.");
				return Ok(_mapper.Map<OrderResponseDTO>(order));
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting pending orders for shipper with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "Shipper")]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetNumberOfOrder(string id)
		{
			try
			{
				_logger.LogInfo($"Attempting to get the number of orders for shipper with ID {id}...");
				DateTime currentDate = DateTime.Now.Date;

				// Orders finished today
				var ordersToday = await _orderRepository
					.FindAll(x => x.ShipperId == id &&
								   x.OrderStatus == OrderStatus.Finish &&
								   x.UpdatedAt.Date == currentDate)
					.ToListAsync();

				// Orders finished this week
				DateTime startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
				var ordersThisWeek = await _orderRepository
					.FindAll(x => x.ShipperId == id &&
								   x.OrderStatus == OrderStatus.Finish &&
								   x.UpdatedAt.Date >= startOfWeek)
					.ToListAsync();

				// Orders finished this month
				var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
				var ordersThisMonth = await _orderRepository
					.FindAll(x => x.ShipperId == id &&
								   x.OrderStatus == OrderStatus.Finish &&
								   x.UpdatedAt.Date >= startOfMonth)
					.ToListAsync();

				// Orders finished this year
				DateTime startOfYear = new DateTime(currentDate.Year, 1, 1);
				var ordersThisYear = await _orderRepository
					.FindAll(x => x.ShipperId == id &&
								   x.OrderStatus == OrderStatus.Finish &&
								   x.UpdatedAt.Date >= startOfYear)
					.ToListAsync();

				// Number of orders for each period
				int numberOfOrdersToday = ordersToday.Count;
				int numberOfOrdersThisWeek = ordersThisWeek.Count;
				int numberOfOrdersThisMonth = ordersThisMonth.Count;
				int numberOfOrdersThisYear = ordersThisYear.Count;

				// Return the results
				var result = new
				{
					Today = numberOfOrdersToday,
					ThisWeek = numberOfOrdersThisWeek,
					ThisMonth = numberOfOrdersThisMonth,
					ThisYear = numberOfOrdersThisYear
				};
				_logger.LogInfo($"Successfully retrieved the number of orders for shipper with ID {id}.");
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting the number of orders for shipper with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "Shipper")]
		[HttpGet("{shipperId}/{year}")]
		public IActionResult GetRevenueShipperPerMonth(string shipperId, int year)
		{
			try
			{
				_logger.LogInfo($"Attempting to get revenue for shipper with ID {shipperId} for the year {year}...");
				List<RevenuePerMonth> revenuePerMonths = _orderRepository.RevenueShipperPerMonth(shipperId, year);
				_logger.LogInfo($"Successfully retrieved revenue for shipper with ID {shipperId} for the year {year}.");
				return Ok(revenuePerMonths);

			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting revenue for shipper with ID {shipperId} for the year {year}: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}
	}
}
