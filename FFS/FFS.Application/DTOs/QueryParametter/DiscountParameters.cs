namespace FFS.Application.DTOs.QueryParametter
{
    public class DiscountParameters : QueryStringParameters
    {
        public int StoreId { get; set; }
        public string? CodeName { get; set; }
    }
}
