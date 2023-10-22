namespace FFS.Application.DTOs.Inventory
{
    public class InventoryDTO
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDelete { get; set; }
    }
}
