using FFS.Application.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.DTOs.FlashSale
{
    public class FlashSaleDTO
    {
		public int Id { get; set; }
		public int StoreId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<FlashSaleDetailDTO>? FlashSaleDetails { get; set; }
		public int? NoOfParticipateFoodSale { get; set; }
		public string? FlashSaleStatus { get; set; }
	}
}
