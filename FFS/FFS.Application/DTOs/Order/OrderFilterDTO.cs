using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.Order
{
    public class OrderFilterDTO
    {
        public int? OrderId { get; set; }
        public int? PageIndex { get; set; }
        public string? CustomerName { get; set; }
        public string? ShipperName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? ToPrice { get; set; }
        public decimal? FromPrice { get; set; }
        public OrderStatus? Status { get; set; }
        public string? SortType { get; set; }

    }
}
