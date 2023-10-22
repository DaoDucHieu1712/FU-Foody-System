namespace FFS.Application.DTOs.Inventory
{
    public class CreateInventoryDTO
    {
        public int FoodId { get; set; }
        public int StoreId { get; set; }
        public int quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDelete { get; set; }

    }
}
