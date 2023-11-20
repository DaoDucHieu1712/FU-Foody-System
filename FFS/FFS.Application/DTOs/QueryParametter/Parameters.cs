using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.QueryParametter {
    public class Parameters {
        public int PageSize { get; set; } = 15;
        public int PageNumber { get; set; } = 1;
        public string ShipperId { get; set; } = string.Empty;
        public OrderStatus OrderStatus { get; set; }

    }
}
