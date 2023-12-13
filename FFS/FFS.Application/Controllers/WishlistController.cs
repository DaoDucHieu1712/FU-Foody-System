using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using FFS.Application.Data;
using FFS.Application.DTOs.Wishlist;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class WishlistController : ControllerBase
	{
		private readonly IWishlistRepository _wishlistRepository;
		private readonly IMapper _mapper;
		private ILoggerManager _logger;
		public WishlistController(IWishlistRepository wishlistRepository, IMapper mapper, ILoggerManager logger)
		{
			_wishlistRepository = wishlistRepository;
			_mapper = mapper;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<List<WishlistDTO>>> GetWishlistByUserId(string userId)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve wishlist for user with ID {userId}.");
				var wishlistItems = await _wishlistRepository.GetListWishlist(userId);

				var wishlistDTOs = new List<WishlistDTO>();

				foreach (var item in wishlistItems)
				{
					var foodId = item.FoodId;
					var isOutOfStock = await _wishlistRepository.IsFoodOutOfStock(foodId);

					wishlistDTOs.Add(new WishlistDTO
					{
						Id = item.Id,
						UserId = item.UserId,
						FoodId = item.FoodId,
						ImageURL = item.Food.ImageURL,
						FoodName = item.Food.FoodName,
						Price = item.Food.Price,
						StoreId = item.Food.StoreId,
						IsOutStock = isOutOfStock
					});
				}
				_logger.LogInfo($"Successfully retrieved wishlist for user with ID {userId}.");
				return Ok(wishlistDTOs);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving wishlist for user with ID {userId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}
		//[HttpPost]
		//public async Task<ActionResult<string>> AddToWishlist(string userId, int foodId)
		//{
		//    try
		//    {
		//        // Call the AddToWishlist method in your repository
		//        bool itemAdded = await _wishlistRepository.AddToWishlist(userId, foodId);

		//        if (itemAdded)
		//        {
		//            return Ok("Item added to the wishlist successfully.");
		//        }
		//        else
		//        {
		//            return BadRequest("Item is already in the wishlist.");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        return StatusCode(500, ex.Message);
		//    }
		//}

		[HttpPost()]
		public async Task<ActionResult> AddToWishlist(string userId, int foodId)
		{
			try
			{
				_logger.LogInfo($"Attempting to add food with ID {foodId} to wishlist for user with ID {userId}.");

				await _wishlistRepository.AddToWishlist(userId, foodId);

				_logger.LogInfo($"Successfully added food with ID {foodId} to wishlist for user with ID {userId}.");

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while adding food with ID {foodId} to wishlist for user with ID {userId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet()]
		public async Task<ActionResult<bool>> IsInWishlist(string userId, int foodId)
		{
			try
			{
				var isInWishlist = await _wishlistRepository.IsInWishlist(userId, foodId);

				return Ok(isInWishlist);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while checking if food with ID {foodId} is in wishlist for user with ID {userId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{wishlistId}")]
		public async Task<ActionResult> RemoveFromWishlist(int wishlistId)
		{
			try
			{
				_logger.LogInfo($"Attempting to remove wishlist item with ID {wishlistId}.");

				await _wishlistRepository.RemoveFromWishlist(wishlistId);
				_logger.LogInfo($"Wishlist item with ID {wishlistId} removed successfully.");

				return Ok("Wishlist item removed successfully.");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while removing wishlist item with ID {wishlistId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}
		 [HttpDelete("{userId}/{foodId}")]
        public async Task<ActionResult> RemoveFromWishlistv2(string userId, int foodId)
        {
            try
            {
                _logger.LogInfo($"Attempting to remove wishlist item for user {userId} and food {foodId}.");

                await _wishlistRepository.RemoveFromWishlist2(userId, foodId);

                _logger.LogInfo($"Wishlist item for user {userId} and food {foodId} removed successfully.");

                return Ok("Wishlist item removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while removing wishlist item for user {userId} and food {foodId}: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }



	}
}
