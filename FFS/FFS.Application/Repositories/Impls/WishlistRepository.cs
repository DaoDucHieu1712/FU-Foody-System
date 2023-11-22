using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Migrations;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
    public class WishlistRepository : EntityRepository<Wishlist, int>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<List<Wishlist>> GetListWishlist(string userId)
        {
            try
            {
                return await GetList(w => w.UserId == userId, w => w.Food, w => w.Food.Store);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsFoodOutOfStock(int foodId)
        {
            try
            {

                int quantity = await _context.Inventories
                    .Where(inventory => inventory.FoodId == foodId)
                    .Select(inventory => inventory.quantity)
                    .FirstOrDefaultAsync();

                return quantity <= 0; // If quantity is less than or equal to 0, it's out of stock

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //public async Task<bool> AddToWishlist(string userId, int foodId)
        //{
        //    try
        //    {
        //        // Check if the item already exists in the wishlist
        //        var existingWishlistItem = await _context.Wishlists
        //            .FirstOrDefaultAsync(w => w.UserId == userId && w.FoodId == foodId);

        //        if (existingWishlistItem != null)
        //        {
        //            // If the item already exists
        //            return false;
        //        }

        //        // If the item doesn't exist in the wishlist
        //        var newWishlistItem = new Wishlist
        //        {
        //            UserId = userId,
        //            FoodId = foodId,
        //            CreatedAt= DateTime.Now,
        //            UpdatedAt = DateTime.Now,
        //            IsDelete = false
        //        };

        //        // Add the new item to the wishlist
        //        await Add(newWishlistItem);

        //        return true; // added successfully
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task AddToWishlist(string userId, int foodId)
        {
            // Implement logic to add the item to the wishlist
            var wishlistItem = new Wishlist { UserId = userId, FoodId = foodId };
            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsInWishlist(string userId, int foodId)
        {
            return await _context.Wishlists.AnyAsync(w => w.UserId == userId && w.FoodId == foodId);
        }

        public async Task RemoveFromWishlist(int wishlistId)
        {
            try
            {
                var wishlist = await FindById(wishlistId);

                if (wishlist != null)
                {
                    await Remove(wishlist);
                }
                else
                {
                    throw new Exception("Wishlist item not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveFromWishlist2(string userId, int foodId)
        {
            
            var wishlistItem = await _context.Wishlists.FirstOrDefaultAsync(w => w.UserId == userId && w.FoodId == foodId);
            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }
        }


    }
}
