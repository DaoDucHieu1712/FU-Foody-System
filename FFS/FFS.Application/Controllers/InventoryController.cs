﻿using AutoMapper;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        public InventoryController(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }
        [HttpGet()]
        public IActionResult GetInventories([FromQuery] InventoryParameters inventoryParameters)
        {
            try
            {
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

                return Ok(
                new
                {
                    entityInventory,
                    metadata
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

		[HttpGet("{fid}")]
		public async Task<IActionResult> GetInventory(int fid)
		{
			try
			{
				var inventory = await _inventoryRepository.FindSingle(x => x.FoodId == fid, x=>x.Food);
				return Ok(_mapper.Map<InventoryDTO>(inventory));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody]  CreateInventoryDTO inventory)
        {
            try
            {
                var existingInventory = await _inventoryRepository.GetInventoryByFoodAndStore(inventory.StoreId, inventory.FoodId);

                if (existingInventory != null)
                {                  
                    return BadRequest("Món ăn này đã có trong tồn kho !");
                }
                await _inventoryRepository.CreateInventory(_mapper.Map<Inventory>(inventory));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


		[HttpPut("{storeId}/{foodId}/{quantity}")]
		public async Task<IActionResult> ImportInventory(int storeId, int foodId, int quantity)
		{
			try
			{
				await _inventoryRepository.ImportInventory(storeId, foodId, quantity);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{storeId}/{foodId}/{quantity}")]
		public async Task<IActionResult> ExportInventory(int storeId, int foodId, int quantity)
		{
			try
			{
				await _inventoryRepository.ExportInventory(storeId, foodId, quantity);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [HttpDelete("{inventoryId}")]
        public async Task<IActionResult> DeleteInventoryByInventoryId(int inventoryId)
        {
            try
            {
                // Call the repository method to delete the inventory
                await _inventoryRepository.DeleteInventoryByInventoryId(inventoryId);
                return Ok("Inventory deleted successfully");
            }
            catch (Exception ex)
            {
                // Handle any errors and return an error response
                return BadRequest($"Error deleting inventory: {ex.Message}");
            }
        }

        [HttpGet("{storeId}/{foodId}")]
        public async Task<ActionResult<bool>> CheckExistingInventory(int storeId, int foodId)
        {
            try
            {
                var existingInventory = await _inventoryRepository.GetInventoryByFoodAndStore(storeId, foodId);
                return existingInventory != null;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
