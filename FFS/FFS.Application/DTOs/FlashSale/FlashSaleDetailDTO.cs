using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.DTOs.FlashSale
{
    public class FlashSaleDetailDTO
    {
        public int FoodId { get; set; }
        public int FlashSaleId { get; set; }
        public decimal? PriceAfterSale { get; set; }
        public int? SalePercent { get; set; }
        public int? NumberOfProductSale { get; set; }
    }
}
