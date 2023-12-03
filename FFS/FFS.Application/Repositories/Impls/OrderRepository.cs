using System.Data;

using AutoMapper;

using Dapper;

using DocumentFormat.OpenXml.Office2010.Excel;

using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Admin;
using FFS.Application.DTOs.Order;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;

using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
	public class OrderRepository : EntityRepository<Order, int>, IOrderRepository
	{
		protected readonly IOrderDetailRepository _orderDetailRepository;
		private readonly IMapper _mapper;
		private readonly DapperContext _dapper;
		public OrderRepository(ApplicationDbContext context, IOrderDetailRepository orderDetailRepository, IMapper mapper, DapperContext dapper) : base(context)
		{
			_orderDetailRepository = orderDetailRepository;
			_mapper = mapper;
			_dapper = dapper;
		}

		public async Task AddOrder(List<OrderDetailDTO> orderDetailDTOs)
		{
			try
			{
				var items = _mapper.Map<List<OrderDetail>>(orderDetailDTOs);
			
				await _orderDetailRepository.AddMultiple(items);
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

		public async Task<OrderDTO> CreateOrder(OrderRequestDTO orderRequestDTO)
		{
			try
			{
				var NewOrder = await CreateAndGetEntity(_mapper.Map<Order>(orderRequestDTO));
				if (NewOrder == null) throw new Exception("Something wrong !");
				return _mapper.Map<OrderDTO>(NewOrder);
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}
		public async Task<int?> GetStoreIdByOrderId(int orderId)
		{
			try
			{
				var order = await _context.Orders
					.Where(o => o.Id == orderId)
					.Select(o => o.OrderDetails.FirstOrDefault().StoreId)
					.FirstOrDefaultAsync();

				return order;
			}
			catch (Exception ex)
			{
				throw new Exception("Error retrieving StoreId by OrderId.", ex);
			}
		}
		public async Task<dynamic> GetOrderDetail(int id)
		{

			try
			{
				dynamic returnData = null;
				var p = new DynamicParameters();
				p.Add("orderId", id);
				using var db = _dapper.connection;

				returnData = await db.QueryAsync<dynamic>("GetOrderDetail", p, commandType: CommandType.StoredProcedure);
				db.Close();
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<List<dynamic>> GetOrder(Parameters parameters)
		{
			try
			{
				dynamic returnData = null;
				var p = new DynamicParameters();
				p.Add("pageSize", parameters.PageSize);
				p.Add("pageNumber", parameters.PageNumber);
				p.Add("shipperId", parameters.ShipperId);
				p.Add("orderStatus", parameters.OrderStatus);

				using var db = _dapper.connection;

				returnData = await db.QueryAsync<dynamic>("GetOrder", p, commandType: CommandType.StoredProcedure);
				db.Close();
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<int> CountGetOrder(Parameters parameters)
		{
			try
			{
				dynamic returnData = null;
				var p = new DynamicParameters();
				p.Add("shipperId", parameters.ShipperId);
				p.Add("orderStatus", parameters.OrderStatus);

				using var db = _dapper.connection;

				returnData = await db.QuerySingleAsync<int>("CountGetOrder", p, commandType: CommandType.StoredProcedure);
				db.Close();
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task CreatePayment(Payment payment)
		{
			await _context.AddAsync(payment);
			await _context.SaveChangesAsync();
		}

		public async Task ConfirmPayment(OrderController.Confirm confirm)
		{
			try
			{
				Order? order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == confirm.OrderId);
				if (order == null)
				{
					throw new Exception("Đơn hàng không tồn tại");
				}
				int? paymentId = order.PaymentId;
				if (paymentId == null)
				{
					throw new Exception("Giao dịch không tồn tại");
				}
				Payment? payment = await _context.Payments.FirstOrDefaultAsync(x => x.Id == paymentId);
				if (payment != null)
				{
					if (confirm.Response == 0)
					{
						payment.Status = Entities.Enum.PaymentStatus.Completed;
					}
					else
					{
						payment.Status = Entities.Enum.PaymentStatus.Cancel;
					}
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public List<OrderStatistic> OrderStatistic(int storeId)
		{
			try
			{
				var orderDetails = _context.OrderDetails
					.Where(od => od.StoreId == storeId)
					.Select(od => od.OrderId)
					.Distinct()
					.ToList();

				var orderStatistics = _context.Orders
					.Where(order => orderDetails.Contains(order.Id))
					.GroupBy(order => order.OrderStatus)
					.Select(group => new OrderStatistic
					{
						OrderStatus = group.Key,
						NumberOfOrder = group.Count()
					})
					.ToList();

				return orderStatistics;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public int CountTotalOrder(int storeId)
		{
			return _context.Orders
				.Include(x => x.OrderDetails)
				.Count(x => x.OrderDetails.Any(od => od.StoreId == storeId));
		}

		public List<FoodDetailStatistic> FoodDetailStatistics(int storeId)
		{
			try
			{
				var foodDetailStatistics = _context.OrderDetails.Include(x => x.Order)
					.Include(x => x.Food)
					.ThenInclude(x => x.Comments)
					.Where(x => x.StoreId == storeId && x.Order.PaymentId != null)
					.GroupBy(x => new { x.Food.FoodName, x.FoodId })
					.Select(g => new FoodDetailStatistic
					{
						FoodName = g.Key.FoodName,
						RateAverage = g.FirstOrDefault(x => x.Food.RateAverage != null).Food.RateAverage,
						RatingCount = g.FirstOrDefault(x => x.Food.RateAverage != null).Food.RatingCount,
						QuantityOfSell = g.Sum(x => x.Quantity)
					})
					.ToList();

				return foodDetailStatistics;
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while retrieving food detail statistics.", ex);
			}
		}

		public List<RevenuePerMonth> RevenuePerMonth(int storeId, int year)
		{
			try
			{
				var monthNames = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
				var distinctOrderIds = _context.OrderDetails
							.Where(od => od.StoreId == storeId)
							.Select(od => od.OrderId)
							.Distinct()
							.ToList();
				var revenuePerMonth = Enumerable.Range(1, 12)
					.Select(month => new RevenuePerMonth
					{
						Month = monthNames[month - 1],
						Revenue = _context.Orders
							.Where(o => o.PaymentId != null && o.CreatedAt.Year == year && o.CreatedAt.Month == month && distinctOrderIds.Contains(o.Id))
							.Sum(x => x.TotalPrice)
					})
					.ToList();

				return revenuePerMonth;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
