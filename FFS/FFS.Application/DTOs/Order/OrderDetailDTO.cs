using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.DTOs.Order
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
