using AutoMapper;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class InventoryController : ControllerBase
	{
		private readonly IInventoryRepository _inventoryRepository;
		private readonly IMapper _mapper;
		private ILoggerManager _logger;

		public InventoryController(IInventoryRepository inventoryRepository, IMapper mapper, ILoggerManager logger)
		{
			_inventoryRepository = inventoryRepository;
			_mapper = mapper;
			_logger = logger;
		}


		[Authorize]
		[HttpGet()]
		public IActionResult GetInventories([FromQuery] InventoryParameters inventoryParameters)
		{
			try
			{
				_logger.LogInfo("Retrieving inventories");
				var Inventories = _inventoryRepository.GetInventories(inventoryParameters);

				var metadata = new
				{
					Inventories.TotalCount,
					Inventories.PageSize,
					Inventories.CurrentPage,
					Inventories.TotalPages,
					Inventories.HasNext,
					Inventories.HasPrevious
				};

				var entityInventory = _mapper.Map<List<InventoryDTO>>(Inventories);
				_logger.LogInfo("Inventories retrieved successfully");
				return Ok(
				new
				{
					entityInventory,
					metadata
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving inventories: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpGet("{fid}")]
		public async Task<IActionResult> GetInventory(int fid)
		{
			try
			{
				_logger.LogInfo($"Retrieving inventory for food with ID: {fid}");
				var inventory = await _inventoryRepository.FindSingle(x => x.FoodId == fid, x => x.Food);
				_logger.LogInfo($"Inventory for food with ID {fid} retrieved successfully");
				return Ok(_mapper.Map<InventoryDTO>(inventory));
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving inventory for food with ID {fid}: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}


		[Authorize(Roles = "StoreOwner")]
		[HttpPost]
		public async Task<IActionResult> CreateInventory([FromBody] CreateInventoryDTO inventory)
		{
			try
			{
				_logger.LogInfo("Creating inventory");
				var existingInventory = await _inventoryRepository.GetInventoryByFoodAndStore(inventory.StoreId, inventory.FoodId);

				if (existingInventory != null)
				{
					_logger.LogInfo("Inventory creation failed: Duplicate inventory found");
					return BadRequest("Món ăn này đã có trong tồn kho !");
				}
				await _inventoryRepository.CreateInventory(_mapper.Map<Inventory>(inventory));
				_logger.LogInfo("Inventory created successfully");
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while creating inventory: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpPut("{storeId}/{foodId}/{quantity}")]
		public async Task<IActionResult> ImportInventory(int storeId, int foodId, int quantity)
		{
			try
			{
				_logger.LogInfo($"Importing inventory for Store ID: {storeId}, Food ID: {foodId}, Quantity: {quantity}");
				await _inventoryRepository.ImportInventory(storeId, foodId, quantity);
				_logger.LogInfo("Inventory import successful");

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while importing inventory: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}


		//[Authorize(Roles = "StoreOwner")]
		[HttpPut("{storeId}/{foodId}/{quantity}")]
		public async Task<IActionResult> ExportInventory(int storeId, int foodId, int quantity)
		{
			try
			{
				_logger.LogInfo($"Exporting inventory for Store ID: {storeId}, Food ID: {foodId}, Quantity: {quantity}");
				await _inventoryRepository.ExportInventory(storeId, foodId, quantity);
				_logger.LogInfo("Inventory export successful");
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while exporting inventory: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpDelete("{inventoryId}")]
		public async Task<IActionResult> DeleteInventoryByInventoryId(int inventoryId)
		{
			try
			{
				_logger.LogInfo($"Deleting inventory with ID: {inventoryId}");
				// Call the repository method to delete the inventory
				await _inventoryRepository.DeleteInventoryByInventoryId(inventoryId);
				_logger.LogInfo("Inventory deleted successfully");
				return Ok("Inventory deleted successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while deleting inventory: {ex.Message}");
				// Handle any errors and return an error response
				return BadRequest($"Error deleting inventory: {ex.Message}");
			}
		}

		[HttpGet("{storeId}/{foodId}")]
		public async Task<ActionResult<bool>> CheckExistingInventory(int storeId, int foodId)
		{
			try
			{
				_logger.LogInfo($"Checking existing inventory for Store ID: {storeId}, Food ID: {foodId}");
				var existingInventory = await _inventoryRepository.GetInventoryByFoodAndStore(storeId, foodId);
				_logger.LogInfo($"Existing inventory check result for Store ID: {storeId}, Food ID: {foodId}: {existingInventory != null}");
				return existingInventory != null;
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while checking existing inventory: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}
	}
}
