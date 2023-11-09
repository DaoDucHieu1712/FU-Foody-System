using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories
{
    public interface IWishlistRepository: IRepository<Wishlist, int>
    {
        Task<List<Wishlist>> GetListWishlist(string userId);
        Task<bool> IsFoodOutOfStock(int foodId);
        Task<bool> AddToWishlist(string userId, int foodId);
        Task RemoveFromWishlist(int wishlistId);
    }
}
