using AutoMapper;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
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
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
           


                return Ok(_mapper.Map<List<InventoryDTO>>(Inventories));
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
                await _inventoryRepository.CreateInventory(_mapper.Map<Inventory>(inventory));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{storeId}/{foodId}/{newQuantity}")]
        public async Task<IActionResult> UpdateInventoryByStoreAndFoodId(int storeId, int foodId, int newQuantity)
        {
            try
            {
                await _inventoryRepository.UpdateInventoryByStoreAndFoodId(storeId, foodId, newQuantity);
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
    }
}
