﻿using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.QueryParametter
{
    public class AllFoodParameters : QueryStringParameters
    {
        public string? CatName { get; set; }
        //Search by food and category
        public string? Search { get; set; }
        public decimal? PriceMin { get; set; } = 0;
        public decimal? PriceMax { get; set; } = 999999999;
        public FilterFood? FilterFood { get; set; }

    }
}
