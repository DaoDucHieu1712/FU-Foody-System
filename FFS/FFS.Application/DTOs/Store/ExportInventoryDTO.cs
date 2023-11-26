namespace FFS.Application.DTOs.Store
{
    public class ExportInventoryDTO
    {
        public int Id { get; set; }
        public int quantity { get; set; }
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public string CategoryName { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
