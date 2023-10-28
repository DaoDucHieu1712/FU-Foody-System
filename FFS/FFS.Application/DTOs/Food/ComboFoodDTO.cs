using System.ComponentModel.DataAnnotations;

namespace FFS.Application.DTOs.Food {
    public class ComboFoodDTO {
        [Required]
        public string Name { get; set; }
        [Required]
        public int StoreId { get; set; }
        public int Percent { get; set; } = 0;
        public List<int>? IdFoods { get; set; }
    }
  
}
