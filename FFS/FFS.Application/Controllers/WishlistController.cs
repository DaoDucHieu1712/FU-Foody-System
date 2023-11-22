using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using FFS.Application.Data;
using FFS.Application.DTOs.Wishlist;
using FFS.Application.Entities;
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
        public WishlistController(IWishlistRepository wishlistRepository, IMapper mapper)
        {
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<WishlistDTO>>> GetWishlistByUserId(string userId)
        {
            try
            {
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

                return Ok(wishlistDTOs);
            }
            catch (Exception ex)
            {
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
            await _wishlistRepository.AddToWishlist(userId, foodId);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult<bool>> IsInWishlist(string userId, int foodId)
        {
            var isInWishlist = await _wishlistRepository.IsInWishlist(userId, foodId);
            return Ok(isInWishlist);
        }

        [HttpDelete("{wishlistId}")]
        public async Task<ActionResult> RemoveFromWishlist(int wishlistId)
        {
            try
            {
                await _wishlistRepository.RemoveFromWishlist(wishlistId);

                return Ok("Wishlist item removed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{userId}/{foodId}")]
        public async Task<ActionResult> RemoveFromWishlistv2(string userId, int foodId)
        {
            await _wishlistRepository.RemoveFromWishlist2(userId, foodId);
            return Ok();
        }



    }
}
