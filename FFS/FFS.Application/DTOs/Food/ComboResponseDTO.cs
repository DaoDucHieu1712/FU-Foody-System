using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.DTOs.Food
{
	public class ComboResponseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int StoreId { get; set; }
		public int Percent { get; set; }
		public string Image { get; set; }
		public decimal Price { get; set; }
		public List<FoodComboDTO> Items { get; set; }
	}
}
