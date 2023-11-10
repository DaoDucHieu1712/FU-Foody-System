namespace FFS.Application.DTOs.Wishlist
{
    public class WishlistDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FoodId { get; set; }
        public string ImageURL { get; set; }
        public string FoodName { get; set; }
        public decimal Price { get; set; }
        public bool IsOutStock { get; set; }
    }
}
