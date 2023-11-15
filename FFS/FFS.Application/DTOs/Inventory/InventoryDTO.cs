namespace FFS.Application.DTOs.Inventory
{
    public class InventoryDTO
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public string ImageURL { get; set; }
        public string CategoryName { get; set; }
        public int quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDelete { get; set; }
    }
}
