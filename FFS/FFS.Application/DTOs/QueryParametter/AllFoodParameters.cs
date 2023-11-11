using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.QueryParametter
{
    public class AllFoodParameters : QueryStringParameters
    {
        public int? CatId { get; set; }
        //Search by food and category
        public string? Search { get; set; }
        public decimal? PriceMin { get; set; } = 0;
        public decimal? PriceMax { get; set; } = 9999;
        public FilterFood? FilterFood { get; set; }

    }
}
