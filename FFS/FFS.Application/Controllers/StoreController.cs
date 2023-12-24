using AutoMapper;

using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class StoreController : ControllerBase
	{

		private readonly IMapper _mapper;
		private readonly IStoreRepository _storeRepository;
		private readonly IFoodRepository _foodRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IComboRepository _comboRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly ILocationRepository _locationRepository;
		private ILoggerManager _logger;


		public StoreController(IMapper mapper, IStoreRepository storeRepository, IFoodRepository foodRepository, ICommentRepository commentRepository, IComboRepository comboRepository, IOrderRepository orderRepository, ILocationRepository locationRepository, ILoggerManager logger)
		{
			_mapper = mapper;
			_storeRepository = storeRepository;
			_foodRepository = foodRepository;
			_commentRepository = commentRepository;
			_comboRepository = comboRepository;
			_orderRepository = orderRepository;
			_locationRepository = locationRepository;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult ListAllStore([FromQuery] AllStoreParameters allStoreParameters)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve all stores...");
				var Stores = _storeRepository.GetAllStores(allStoreParameters);
				_logger.LogInfo($"Retrieved {Stores.TotalCount} stores successfully.");
				var metadata = new
				{
					Stores.TotalCount,
					Stores.PageSize,
					Stores.CurrentPage,
					Stores.TotalPages,
					Stores.HasNext,
					Stores.HasPrevious
				};

				var StoreDTOs = _mapper.Map<List<AllStoreDTO>>(Stores);

				return Ok(
				new
				{
					StoreDTOs,
					metadata
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving all stores: {ex.Message}");
				return BadRequest(ex.Message);
			}

		}
		[HttpGet]
		public async Task<IActionResult> GetTop10Store()
		{
			try
			{
				_logger.LogInfo("Attempting to retrieve the top 10 popular stores...");
				var top10Store = await _storeRepository.GetTop10PopularStore();
				var top10StoreDTO = _mapper.Map<List<AllStoreDTO>>(top10Store);
				_logger.LogInfo("Retrieved top 10 popular stores successfully.");
				return Ok(top10StoreDTO);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving the top 10 popular stores: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetStoreInformation(int id)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve information for store with ID: {id}");
				StoreInforDTO storeInforDTO = await _storeRepository.GetInformationStore(id);
				_logger.LogInfo($"Retrieved information for store with ID: {id} successfully.");
				return Ok(storeInforDTO);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving store information for ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetStoreByUid(string uId)

		{
			try
			{
				var StoreByUid = await _storeRepository.FindSingle(x => x.UserId == uId);

				return Ok(StoreByUid);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{uId}")]
		public async Task<IActionResult> GetStore(string uId)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve store information for User ID: {uId}");
				var store = await _storeRepository.FindSingle(x => x.UserId == uId);
				var location = await _locationRepository.FindSingle(x => x.UserId == uId);
				_logger.LogInfo($"Retrieved store information for User ID: {uId} successfully.");
				return Ok(new { store, location });
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving store information for User ID {uId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpGet("exportfood")]
		public async Task<IActionResult> ExportFood(int id)
		{
			try
			{
				_logger.LogInfo($"Attempting to export food data for Store ID: {id}");
				var data = await _storeRepository.ExportFood(id);
				string uniqueFileName = "ThongKe_MonAn_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
				_logger.LogInfo($"Exported food data for Store ID: {id} successfully.");
				return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while exporting food data for Store ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpGet("exportinventory")]
		public async Task<IActionResult> ExportInventory(int id)
		{
			try
			{
				_logger.LogInfo($"Attempting to export inventory data for Store ID: {id}");
				var data = await _storeRepository.ExportInventory(id);
				string uniqueFileName = "ThongKe_Kho_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

				return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while exporting inventory data for Store ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateStore(int id, StoreInforDTO storeInforDTO)
		{
			try
			{
				_logger.LogInfo($"Attempting to update information for Store ID: {id}");
				StoreInforDTO inforDTO = await _storeRepository.UpdateStore(id, storeInforDTO);
				_logger.LogInfo($"Updated store information for Store ID: {id} successfully.");
				return Ok(inforDTO);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while updating store information for Store ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> DetailStore(int id)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve detailed information for Store ID: {id}");
				StoreInforDTO storeInforDTO = await _storeRepository.GetDetailStore(id);
				List<Combo> combos = await _comboRepository.GetList(x => x.StoreId == id && x.IsDelete == false);
				storeInforDTO.Combos = combos;
				_logger.LogInfo($"Retrieved detailed information for Store ID: {id} successfully.");
				return Ok(storeInforDTO);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving detailed information for Store ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{rate}/{id}")]
		[Authorize]
		public async Task<IActionResult> GetCommentByStore(int rate, int id)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve comments for Store ID: {id} with rating: {rate}");
				dynamic comment = await _storeRepository.GetCommentByStore(rate, id);
				_logger.LogInfo($"Retrieved comments for Store ID: {id} with rating: {rate} successfully.");
				return Ok(comment);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving comments for Store ID {id} with rating {rate}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> GetCommentReply(int id)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve comment replies for Comment ID: {id}");
				dynamic comment = await _storeRepository.GetCommentReply(id);
				_logger.LogInfo($"Retrieved comment replies for Comment ID: {id} successfully.");
				return Ok(comment);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving comment replies for Comment ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{idShop}/{idCategory}")]
		public async Task<IActionResult> GetFoodByCategory(int idShop, int idCategory)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve food for Shop ID: {idShop} and Category ID: {idCategory}");
				List<FoodDTO> foodDTOs = await _storeRepository.GetFoodByCategory(idShop, idCategory);
				_logger.LogInfo($"Retrieved food items for Shop ID: {idShop} and Category ID: {idCategory} successfully.");
				return Ok(foodDTOs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet]
		public IActionResult GetFoodByName([FromQuery] string? name)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve food by name: {name}");
				List<Food> foods;

				if (string.IsNullOrEmpty(name))
				{
					// If the name is empty or null, return all items.
					foods = _foodRepository.FindAll().ToList();
				}
				else
				{
					// If the name is not empty, perform the search.
					foods = _foodRepository.FindAll(i => i.FoodName.Contains(name)).ToList();
				}
				List<FoodDTO> foodDTOs = _mapper.Map<List<FoodDTO>>(foods);
				_logger.LogInfo($"Retrieved {foodDTOs.Count} food items matching the search criteria.");
				return Ok(foodDTOs);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving food by name {name}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> RatingStore([FromBody] StoreRatingDTO storeRatingDTO)
		{
			try
			{
				_logger.LogInfo($"Attempting to rate the store with ID {storeRatingDTO.StoreId}.");
				Comment c = _mapper.Map<Comment>(storeRatingDTO);

				await _commentRepository.RatingStore(c);
				if (storeRatingDTO.ParentCommentId != null)
				{
					dynamic comment = await _storeRepository.GetCommentReply(Convert.ToInt32(storeRatingDTO.ParentCommentId));
					_logger.LogInfo($"Successfully rated the store with ID {storeRatingDTO.StoreId}. Returning comment reply.");
					return Ok(comment);
				}

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while rating the store with ID {storeRatingDTO.StoreId}: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpGet("{storeId}")]
		public IActionResult OrderStatistic(int storeId)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve order statistics for store with ID {storeId}.");
				List<OrderStatistic> orderStatistics = _orderRepository.OrderStatistic(storeId);
				_logger.LogInfo($"Successfully retrieved order statistics for store with ID {storeId}.");
				return Ok(new
				{
					TotalOrder = _orderRepository.CountTotalOrder(storeId),
					OrdersStatistic = orderStatistics
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving order statistics for store with ID {storeId}: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}

		}

		[Authorize(Roles = "StoreOwner")]
		[HttpGet("{storeId}")]
		public IActionResult GetFoodDetailStatistics(int storeId)
		{
			try
			{
				List<FoodDetailStatistic> foodDetailStatistics = _orderRepository.FoodDetailStatistics(storeId);
				return Ok(foodDetailStatistics);

			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}

		}

		[Authorize(Roles = "StoreOwner")]
		[HttpGet("{storeId}/{year}")]
		public IActionResult GetRevenuePerMonth(int storeId, int year)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve food detail statistics for store with ID {storeId}.");
				List<RevenuePerMonth> revenuePerMonths = _orderRepository.RevenuePerMonth(storeId, year);
				_logger.LogInfo($"Successfully retrieved food detail statistics for store with ID {storeId}.");
				return Ok(revenuePerMonths);

			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving food detail statistics for store with ID {storeId}: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}

		//[Authorize(Roles = "StoreOwner")]
		[HttpGet("{storeId}")]
		public IActionResult ExportOrder(int storeId)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve food detail statistics for store with ID {storeId}.");
				var data = _orderRepository.ExportOrder(storeId);
				string uniqueFileName = "ThongKe_DonHang_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
				_logger.LogInfo($"Successfully retrieved food detail statistics for store with ID {storeId}.");
				return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);

			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving food detail statistics for store with ID {storeId}: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}
	}
}
