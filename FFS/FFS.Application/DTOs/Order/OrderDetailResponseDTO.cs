namespace FFS.Application.DTOs.Order
{
	public class OrderDetailResponseDTO
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int StoreId { get; set; }
		public string StoreName { get; set; }
		public int? FoodId { get; set; }
		public string? FoodName { get; set; }
		public int? ComboId { get; set; }
		public string? ComboName { get; set; }
        public string ImageURL { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
