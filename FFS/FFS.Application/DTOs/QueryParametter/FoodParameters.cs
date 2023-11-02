namespace FFS.Application.DTOs.QueryParametter
{
    public class FoodParameters : QueryStringParameters
    {
        public string uId { get; set; }
        public string? FoodName { get; set; }
    }
}
